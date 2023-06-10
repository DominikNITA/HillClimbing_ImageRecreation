using System;
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
            Size = new Size(size.Width, size.Width);   
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            using (Matrix m = new Matrix())
            {
                graphics.FillEllipse(new SolidBrush(Color), Position.X, Position.Y, Size.Width, Size.Width);
            }
        }
    }
}
