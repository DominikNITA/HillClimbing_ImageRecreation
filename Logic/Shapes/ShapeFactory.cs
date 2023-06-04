using Logic.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Shapes
{
    internal class ShapeFactory
    {
        AlgorithmParameters _algorithmParameters;
        int _bitmapWidth, _bitmapHeight;
        Random _random;

        public ShapeFactory(AlgorithmParameters algorithmParameters, int bitmapWidth, int bitmapHeight)
        {
            _algorithmParameters = algorithmParameters;
            _bitmapWidth = bitmapWidth;
            _bitmapHeight = bitmapHeight;
            _random = new Random();
        }

        public IShape CreateRandomShape()
        {
            int index = _random.Next(_algorithmParameters.Shapes.Count());

            var (color, isBackgroundColor) = GetRandomShapeColor();
            var size = GetRandomShapeSize();
            var position = GetRandomShapePosition(size);
            var rotation = GetRandomRotation();

            return (IShape)Activator.CreateInstance(_algorithmParameters.Shapes.ElementAt(index),
                color,
                size,
                position,
                rotation,
                isBackgroundColor);
        }

        private (Color color, bool isBackgroundColor) GetRandomShapeColor()
        {
            if (_random.NextDouble() < _algorithmParameters.UseBackgroundColorChance)
            {
                return (_algorithmParameters.BackgroundColor, true);
            }

            return (Color.FromArgb(
                _algorithmParameters.AllowAlpha ? _random.Next(1, 256) : 255,
                _random.Next(0, 256),
                _random.Next(0, 256),
                _random.Next(0, 256)
                ), false);
        }

        private Point GetRandomShapePosition(Size shapeSize)
        {
            return new Point(
                _random.Next(0 - (shapeSize.Width / 2), _bitmapWidth + (shapeSize.Width / 2)),
                _random.Next(0 - (shapeSize.Height / 2), _bitmapHeight + (shapeSize.Height / 2))
                );
        }

        private Size GetRandomShapeSize()
        {
            return new Size(
                _random.Next(_algorithmParameters.MinShapeSize, _algorithmParameters.MaxShapeSize),
                _random.Next(_algorithmParameters.MinShapeSize, _algorithmParameters.MaxShapeSize)
                );
        }

        private float GetRandomRotation()
        {
            if (_algorithmParameters.AllowRotation == false)
            {
                return 0;
            }
            return _random.Next(0, 360);
        }
    }
}
