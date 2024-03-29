﻿using Logic.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Shapes
{
    internal class ShapeFactory
    {
        AlgorithmParameters _algorithmParameters;
        int _bitmapWidth, _bitmapHeight;
        Random _random;
        ColorBucketManager? _colorBucketManager;

        public ShapeFactory(AlgorithmParameters algorithmParameters, int bitmapWidth, int bitmapHeight, ColorBucketManager? colorBucketManager)
        {
            _algorithmParameters = algorithmParameters;
            _bitmapWidth = bitmapWidth;
            _bitmapHeight = bitmapHeight;
            _random = new Random();
            _colorBucketManager = colorBucketManager;
        }

        public IShape? CreateRandomShape(int currentIteration)
        {
            int index = _random.Next(_algorithmParameters.Shapes.Count());

            var (color, isBackgroundColor) = GetRandomShapeColor(currentIteration);
            var size = GetRandomShapeSize();
            var position = GetRandomShapePosition(size);
            var rotation = GetRandomRotation();

            return CreateShape(_algorithmParameters.Shapes.ElementAt(index),
                color,
                size,
                position,
                rotation,
                isBackgroundColor);
        }

        public static IShape? CreateShape(Type shapeType, Color color, Size size, Point position, float rotation, bool isBackgroundColor)
        {
            return (IShape?)Activator.CreateInstance(shapeType,
                color,
                size,
                position,
                rotation,
                isBackgroundColor);
        }

        private (Color color, bool isBackgroundColor) GetRandomShapeColor(int currentIteration)
        {
            if (ShouldUseBackgroundColor())
            {
                return (_algorithmParameters.BackgroundColor, true);
            }

            if (ShouldUseColorDict(currentIteration))
            {
                var randomColor = _colorBucketManager!.GetRandomColor(0.0005);
                return (Color.FromArgb(
                     _algorithmParameters.AllowAlpha ? _random.Next(1, 256) : 255,
                     randomColor.R,
                     randomColor.G,
                     randomColor.B
                     ), false);
            }

            return (Color.FromArgb(
                _algorithmParameters.AllowAlpha ? _random.Next(1, 256) : 255,
                _random.Next(0, 256),
                _random.Next(0, 256),
                _random.Next(0, 256)
                ), false);
        }

        private bool ShouldUseBackgroundColor()
        {
            return _random.NextDouble() < _algorithmParameters.UseBackgroundColorChance;
        }

        private bool ShouldUseColorDict(int currentIteration)
        {
            return _algorithmParameters.ColorDictParameters.Enabled &&
                currentIteration >= _algorithmParameters.ColorDictParameters.StartUsingFromIteration &&
                _random.NextDouble() < _algorithmParameters.ColorDictParameters.UseColorDictChance;
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
