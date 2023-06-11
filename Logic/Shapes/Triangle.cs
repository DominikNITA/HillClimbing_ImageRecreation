using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Shapes
{
    public class Triangle : Shape
    {
        public new static string DisplayName { get; } = "Triangle";
        public Triangle(Color color, Size size, Point position, float rotation, bool isUsingBackgroundColor) : base(color, size, position, rotation, isUsingBackgroundColor) { }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            using (Matrix m = new Matrix())
            {
                graphics.TranslateTransform(Position.X, Position.Y);
                graphics.RotateTransform(Rotation);
                var trianglePoints = new Point[]
                {
                        new(0, -_halfHeight),
                        new(-_halfWidth, _halfHeight),
                        new(_halfWidth, _halfHeight),
                };
                graphics.FillPolygon(new SolidBrush(Color), trianglePoints);
                graphics.ResetTransform();
            }
        }
    }
}
