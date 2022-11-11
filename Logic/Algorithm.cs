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

        public AlgorithmResult Calculate(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                _currentIteration++;
                int attempts = 0;
                while (Calculate() == false)
                {
                    attempts++;
                    if (attempts >= 10000)
                    {
                        throw new Exception("Could not find better solution in 100 attempts");
                    }
                }
                if (_currentIteration % 50 == 0)
                {
                    Console.WriteLine($"Score for iteration {_currentIteration}: " + GetScore(_lastImageBitmap,
                             new Size() { Height = _targetImageBitmap.Height, Width = _targetImageBitmap.Width },
                             new Point() { X = 0, Y = 0 }
                            ));
                }
            }
            return CalculateResult();
        }

        private bool Calculate()
        {
            Shape shapeToDraw = GetRandomShapeToDraw(_parameters);
            Size shapeSize = GetRandomShapeSize(_parameters);
            Point shapePosition = GetRandomShapePosition(_targetImageBitmap, shapeSize);
            Color shapeColor = GetRandomShapeColor();

            Bitmap currentIterationBitmap = DrawShape(shapeToDraw, shapeSize, shapePosition, shapeColor);
            if (CompareScores(currentIterationBitmap, _lastImageBitmap, shapeSize, shapePosition) < 0)
            {
                _lastImageBitmap = currentIterationBitmap;
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

        private double CalculateScoreForPixel(int x, int y, Color pixelColor)
        {
            var targetPixel = _targetImageBitmap.GetPixel(x, y);
            double score = 0;
            score += Math.Pow(Math.Abs((double)targetPixel.A - pixelColor.A), 2);
            score += Math.Pow(Math.Abs((double)targetPixel.R - pixelColor.R), 2);
            score += Math.Pow(Math.Abs((double)targetPixel.G - pixelColor.G), 2);
            score += Math.Pow(Math.Abs((double)targetPixel.B - pixelColor.B), 2);

            return score;
        }

        private Bitmap DrawShape(Shape shapeToDraw, Size shapeSize, Point shapePosition, Color shapeColor)
        {
            Bitmap currentIterationBitmap = new(_lastImageBitmap);
            using (Graphics currentIterationGraphics = Graphics.FromImage(currentIterationBitmap))
            {
                if (shapeToDraw == Shape.Ellipse)
                {
                    currentIterationGraphics.FillEllipse(new SolidBrush(shapeColor), shapePosition.X, shapePosition.Y, shapeSize.Width, shapeSize.Height);
                }
                else if (shapeToDraw == Shape.Rectangle)
                {
                    currentIterationGraphics.FillRectangle(new SolidBrush(shapeColor), new Rectangle(shapePosition.X, shapePosition.Y, shapeSize.Width, shapeSize.Height));
                }
                var pathToImage = $"{_pathToStorage}\\{_currentIteration}.png";
                currentIterationBitmap.Save(pathToImage);
                return currentIterationBitmap;
            }
        }

        private Color GetRandomShapeColor()
        {
            return Color.FromArgb(
                _random.Next(20, 256),
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
            var pathToImage = $"{_pathToStorage}\\{_currentIteration}.png";
            _lastImageBitmap.Save(pathToImage);
            return new AlgorithmResult()
            {
                Iteration = _currentIteration,
                PathToImage = pathToImage,
                Score = GetScore(_lastImageBitmap,
                         new Size() { Height = _targetImageBitmap.Height, Width = _targetImageBitmap.Width },
                         new Point() { X = 0, Y = 0 }
                        )
            };
        }

        private void Initialize()
        {
            _targetImageBitmap = new(_pathToTargetImage);
            _lastImageBitmap = new(_targetImageBitmap.Width, _targetImageBitmap.Height, PixelFormat.Format32bppArgb);

            Graphics initialGraphics = Graphics.FromImage(_lastImageBitmap);
            initialGraphics.FillRectangle(new SolidBrush(_parameters.BackgroundColor), new Rectangle(0, 0, _targetImageBitmap.Width, _targetImageBitmap.Height));
            initialGraphics.FillEllipse(new SolidBrush(Color.GreenYellow), 150, 250, 30, 60);

            _pathToStorage = Path.Combine(_pathToStorage, DateTime.Now.Ticks.ToString() + Guid.NewGuid().ToString());
            Directory.CreateDirectory(_pathToStorage);

            _lastImageBitmap.Save($"{_pathToStorage}\\{_currentIteration}.png");

            initialGraphics.Dispose();
        }
    }
}
