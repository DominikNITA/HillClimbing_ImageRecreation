using Logic.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Algorithm
    {
        AlgorithmParameters _parameters;
        string _pathToStorage;

        string _pathToTargetImage;
        Bitmap _targetImageBitmap;

        double _lastScore = -1;
        string _pathToLastImage = "";
        Bitmap _lastImageBitmap;

        int _currentIteration = 0;

        Random _random;
        public Algorithm(AlgorithmParameters parameters, string pathToTargetImage, string pathToStorage)
        {
            _parameters = parameters;
            _pathToTargetImage = pathToTargetImage;
            _pathToStorage = pathToStorage;
            _random = new Random();
            Initialize();
        }

        public AlgorithmResult CalculateNextImage()
        {
            _currentIteration++;
            Console.WriteLine($"Calculating iteration: {_currentIteration}");
            int attempts = 0;
            while (NextIteration() == false)
            {
                attempts++;
                if (attempts >= 10000)
                {
                    throw new Exception("Could not find better solution in 100 attempts");
                }
            }

            return CalculateResult();
        }

        private bool NextIteration()
        {
            Shape shapeToDraw = GetRandomShapeToDraw(_parameters);
            Size shapeSize = GetRandomShapeSize(_parameters);
            Point shapePosition = GetRandomShapePosition(_targetImageBitmap, shapeSize);
            Color shapeColor = GetRandomShapeColor();

            Bitmap currentIterationBitmap = DrawShape(shapeToDraw, shapeSize, shapePosition, shapeColor);

            var scoreDifference = CompareScores(currentIterationBitmap, _lastImageBitmap, shapeSize, shapePosition);
            if (scoreDifference < 0)
            {
                _lastImageBitmap = currentIterationBitmap;
                _lastScore += scoreDifference;
                return true;
            }
            currentIterationBitmap.Dispose();
            return false;
        }

        private double CompareScores(Bitmap currentIterationBitmap, Bitmap lastImageBitmap, Size shapeSize, Point shapePosition)
        {
            return GetScore(currentIterationBitmap, shapeSize, shapePosition) - GetScore(lastImageBitmap, shapeSize, shapePosition);
        }

        private double GetScore(Bitmap image, Size shapeSize, Point shapePosition)
        {
            float multiplier = 1.4f;
            double score = 0;
            for (int x = shapePosition.X; x < shapePosition.X + shapeSize.Width; x++)
            {
                for (int y = shapePosition.Y; y < shapePosition.Y + shapeSize.Height; y++)
                {
                    if (x < 0 || y < 0 || x >= image.Width || y >= image.Height)
                    {
                        continue;
                    }
                    var pixel = image.GetPixel(x, y);
                    score += CalculateScoreForPixel(x, y, pixel);
                }
            }
            return score;
        }

        /// <summary>
        /// Calculates the score for given pixel. Best score 0, worst score 4
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="pixelColor"></param>
        /// <returns></returns>
        private double CalculateScoreForPixel(int x, int y, Color pixelColor)
        {
            var targetPixel = _targetImageBitmap.GetPixel(x, y);
            double score = 0;

            if (targetPixel.A == 0)
            {
                // If both pixels are completly transparent we don't compare other colors
                if (pixelColor.A == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Abs((targetPixel.A - pixelColor.A) / (double)255);
                }
            }
            score += Math.Abs((targetPixel.A - pixelColor.A) / (double)255);
            score += Math.Abs((targetPixel.R - pixelColor.R) / (double)255);
            score += Math.Abs((targetPixel.G - pixelColor.G) / (double)255);
            score += Math.Abs((targetPixel.B - pixelColor.B) / (double)255);

            return score;
        }

        private Bitmap DrawShape(Shape shapeToDraw, Size shapeSize, Point shapePosition, Color shapeColor)
        {
            Bitmap currentIterationBitmap = new(_lastImageBitmap);
            using (Graphics currentIterationGraphics = Graphics.FromImage(currentIterationBitmap))
            {
                if (_random.NextDouble() < _parameters.UseBackgroundColorChance)
                {
                    currentIterationGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    shapeColor = _parameters.BackgroundColor;
                }

                var x = shapePosition.X;
                var y = shapePosition.Y;
                var width = shapeSize.Width;
                var height = shapeSize.Height;
                var brush = new SolidBrush(shapeColor);

                if (shapeToDraw == Shape.Ellipse)
                {
                    currentIterationGraphics.FillEllipse(brush, x, y, width, height);
                }
                else if (shapeToDraw == Shape.Circle)
                {
                    currentIterationGraphics.FillEllipse(brush, x, y, width, width);
                }
                else if (shapeToDraw == Shape.Triangle)
                {
                    var trianglePoints = new Point[]
                    {
                        new(x, y),
                        new(x + width, y),
                        new(x + width/2, y + height),
                    };
                    currentIterationGraphics.FillPolygon(brush, trianglePoints);
                }
                else if (shapeToDraw == Shape.Rectangle)
                {
                    currentIterationGraphics.FillRectangle(brush, new Rectangle(x, y, width, height));
                }
                brush.Dispose();

                //var pathToImage = $"{_pathToStorage}\\{_currentIteration}.png";
                //currentIterationBitmap.Save(pathToImage);
                return currentIterationBitmap;
            }
        }

        private Color GetRandomShapeColor()
        {
            return Color.FromArgb(
                _parameters.AllowAlpha ? _random.Next(1, 256) : 255,
                _random.Next(0, 256),
                _random.Next(0, 256),
                _random.Next(0, 256)
                );
        }

        private Point GetRandomShapePosition(Bitmap bitmap, Size shapeSize)
        {
            return new Point(
                _random.Next(0 - (shapeSize.Width / 2), bitmap.Width + (shapeSize.Width / 2)),
                _random.Next(0 - (shapeSize.Height / 2), bitmap.Height + (shapeSize.Height / 2))
                );
        }

        private Size GetRandomShapeSize(AlgorithmParameters parameters)
        {
            return new Size(
                _random.Next(parameters.MinShapeSize, parameters.MaxShapeSize),
                _random.Next(parameters.MinShapeSize, parameters.MaxShapeSize)
                );
        }

        private Shape GetRandomShapeToDraw(AlgorithmParameters parameters)
        {
            if (parameters == null || parameters.Shapes == null)
            {
                return Shape.Ellipse;
            }
            return parameters.Shapes.ElementAt(_random.Next(0, parameters.Shapes.Count()));
        }

        private AlgorithmResult CalculateResult()
        {
            //if (_currentIteration >= _parameters.MaxIterations)
            //{
            //    UpdateLastScore();
            //}

            var pathToImage = $"{_pathToStorage}\\{_currentIteration}.png";
            if (_currentIteration % _parameters.ImagePresentationInterval == 0 ||
                _currentIteration < 20)
            {
                _lastImageBitmap.Save(pathToImage);
            }
            else
            {
                pathToImage = null;
            }
            Console.WriteLine(_lastScore);
            return new AlgorithmResult()
            {
                Iteration = _currentIteration,
                PathToImage = pathToImage,
                Score = _lastScore
            };
        }

        private void Initialize()
        {
            _targetImageBitmap = new(_pathToTargetImage);
            _lastImageBitmap = new(_targetImageBitmap.Width, _targetImageBitmap.Height, PixelFormat.Format32bppArgb);

            Graphics initialGraphics = Graphics.FromImage(_lastImageBitmap);
            initialGraphics.FillRectangle(new SolidBrush(_parameters.BackgroundColor), new Rectangle(0, 0, _targetImageBitmap.Width, _targetImageBitmap.Height));

            _pathToStorage = Path.Combine(_pathToStorage, DateTime.Now.Ticks.ToString() + Guid.NewGuid().ToString());
            Directory.CreateDirectory(_pathToStorage);

            _lastImageBitmap.Save($"{_pathToStorage}\\{_currentIteration}.png");

            initialGraphics.Dispose();

            _lastScore = GetScore(_lastImageBitmap,
             new Size() { Height = _targetImageBitmap.Height, Width = _targetImageBitmap.Width },
             new Point() { X = 0, Y = 0 }
            );

            new VideoGenerator().Test();
        }

        public int GetMaxIterations()
        {
            return _parameters.MaxIterations;
        }
    }
}
