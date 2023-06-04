﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Shapes
{
    public class Circle : Shape
    {
        public new static string DisplayName { get; } = "Circle";
        public Circle(Color color, Size size, Point position, float rotation, bool isUsingBackgroundColor) : base(color, size, position, rotation, isUsingBackgroundColor)
        {
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            using (Matrix m = new Matrix())
            {
                m.RotateAt(Rotation, new PointF(Position.X + (Size.Width / 2), Position.Y - (Size.Height / 2)));
                graphics.Transform = m;
                graphics.FillEllipse(new SolidBrush(Color), Position.X, Position.Y, Size.Width, Size.Width);
                graphics.ResetTransform();
            }
        }
    }
}