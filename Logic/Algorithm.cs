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
        Bitmap? _lastImageBitmap;

        int _currentIteration = 0;
        public Algorithm(AlgorithmParameters parameters, string pathToTargetImage, string pathToStorage)
        {
            _parameters = parameters;
            _pathToTargetImage = pathToTargetImage;
            _pathToStorage = pathToStorage;
            Initialize();
        }

        public AlgorithmResult Calculate(int iterations)
        {
            Calculate();
            return CalculateResult();
        }

        private void Calculate()
        {
            throw new NotImplementedException();
        }

        private AlgorithmResult CalculateResult()
        {
            throw new NotImplementedException();
        }

        private void Initialize()
        {
            _targetImageBitmap = new(_pathToTargetImage);
            _lastImageBitmap = new(_targetImageBitmap.Width, _targetImageBitmap.Height, PixelFormat.Format32bppArgb);

            Graphics initialGraphics = Graphics.FromImage(_lastImageBitmap);
            initialGraphics.FillRectangle(new SolidBrush(_parameters.BackgroundColor), new Rectangle(0, 0, _targetImageBitmap.Width, _targetImageBitmap.Height));

            _pathToStorage = Path.Combine(_pathToStorage, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_pathToStorage);

            _lastImageBitmap.Save(_pathToStorage + $"\\{_currentIteration}.png");

            initialGraphics.Dispose();
        }
    }
}
