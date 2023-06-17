using Logic.Helpers;
using Logic.Models;
using Logic.ScoreCalculator;
using Logic.Shapes;
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

        string _pathToTargetImage;
        Bitmap _targetImageBitmap;

        double _lastScore = -1;
        Bitmap _latestImageBitmap;

        int _currentIteration = 0;

        ShapeFactory _shapeFactory;

        IScoreCalculator _scoreCalculator;

        public string Id { get; private set; }

        const int MAX_ATTEMPTS_PER_ITERATION = 10000;

        public Algorithm(AlgorithmParameters parameters, string pathToTargetImage)
        {
            _parameters = parameters;
            _pathToTargetImage = pathToTargetImage;
            _scoreCalculator = new BasicScoreCalculator();
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
                if (attempts >= MAX_ATTEMPTS_PER_ITERATION)
                {
                    throw new Exception($"Could not find better solution in {MAX_ATTEMPTS_PER_ITERATION} attempts");
                }
            }

            return CalculateResult();
        }

        private bool NextIteration()
        {
            IShape shapeToDraw = _shapeFactory.CreateRandomShape();

            Bitmap currentIterationBitmap = new(_latestImageBitmap);
            using (Graphics currentIterationGraphics = Graphics.FromImage(currentIterationBitmap))
            {
                shapeToDraw.Draw(currentIterationGraphics);
            }

            var scoreDifference = _scoreCalculator.CompareScores(currentIterationBitmap, _latestImageBitmap, shapeToDraw, _targetImageBitmap);
            if (scoreDifference < 0)
            {
                _latestImageBitmap = currentIterationBitmap;
                _lastScore += scoreDifference;
                currentIterationBitmap.Dispose();
                return true;
            }
            currentIterationBitmap.Dispose();
            return false;
        }

        private AlgorithmResult CalculateResult()
        {
            var pathToImage = StorageHelper.GetPathForIterationImage(Id, _currentIteration);
            if (_currentIteration % _parameters.ImagePresentationInterval == 0 ||
                _currentIteration < 20)
            {
                _latestImageBitmap.Save(pathToImage);
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
            InitializeBackgroundImage();

            Id = DateTime.Now.Ticks.ToString() + Guid.NewGuid().ToString();
            Directory.CreateDirectory(StorageHelper.GetPathForIterationsFolderById(Id));
            _latestImageBitmap.Save(StorageHelper.GetPathForIterationImage(Id, _currentIteration));

            _lastScore = _scoreCalculator.GetScoreForWholeImage(_latestImageBitmap, _targetImageBitmap);

            _shapeFactory = new ShapeFactory(_parameters, _targetImageBitmap.Width, _targetImageBitmap.Height);
        }

        private void InitializeBackgroundImage()
        {
            _latestImageBitmap = new(_targetImageBitmap.Width, _targetImageBitmap.Height, PixelFormat.Format32bppArgb);
            Graphics initialGraphics = Graphics.FromImage(_latestImageBitmap);
            initialGraphics.FillRectangle(new SolidBrush(_parameters.BackgroundColor), new System.Drawing.Rectangle(0, 0, _targetImageBitmap.Width, _targetImageBitmap.Height));
            initialGraphics.Dispose();
        }

        public int GetMaxIterations()
        {
            return _parameters.MaxIterations;
        }

        public static double GetScorePercentage(double score, int? width, int? height)
        {
            if(width == null || height == null)
            {
                return 0;
            }
            return (1 - (score) / ((double)width * (double)height * 4)) * 100;
        }
    }
}
