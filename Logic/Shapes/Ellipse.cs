using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Shapes
{
    public class Ellipse : Shape
    {
        public new static string DisplayName { get; set; } = "Ellipse";
        public Ellipse(Color color, Size size, Point position, float rotation, bool isUsingBackgroundColor) : base(color, size, position, rotation, isUsingBackgroundColor)
        {
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            using (Matrix m = new Matrix())
            {
                graphics.TranslateTransform(Position.X, Position.Y);
                graphics.RotateTransform(Rotation);
                graphics.FillEllipse(new SolidBrush(Color), -_halfWidth, -_halfHeight, Size.Width, Size.Height);
                graphics.ResetTransform();
            }
        }
    }
}
