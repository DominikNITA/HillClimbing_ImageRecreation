using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace Logic.Shapes
{
    public abstract class Shape : IShape
    {
        public static string DisplayName { get; } = "Generic Shape";

        public bool IsUsingBackgroundColor { get; init; }
        public Color Color { get; init; }
        public Size Size { get; init; }
        public Point Position { get; init; }
        public float Rotation { get; init; }

        protected int _halfWidth => Size.Width / 2;
        protected int _halfHeight => Size.Height / 2;

        public Shape(Color color, Size size, Point position, float rotation, bool isUsingBackgroundColor)
        {
            Color = color;
            Size = size;
            Position = position;
            Rotation = rotation;
            IsUsingBackgroundColor = isUsingBackgroundColor;
        }

        public virtual void Draw(Graphics graphics)
        {
            if (IsUsingBackgroundColor)
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            }
        }

        public IEnumerable<Point> GetModifiedPixels(Bitmap image)
        {
            //int x1, x2, y1, y2;
            //x1 = (int)(Position.X - _halfWidth * Math.Cos(AngleToDegree(Rotation)) + _halfHeight * Math.Sin(AngleToDegree(Rotation)));
            //x2 = (int)(Position.X + _halfWidth * Math.Cos(AngleToDegree(Rotation)) - _halfHeight * Math.Sin(AngleToDegree(Rotation)));
            //y1 = (int)(Position.Y - _halfWidth * Math.Cos(AngleToDegree(Rotation)) + _halfHeight * Math.Sin(AngleToDegree(Rotation)));
            //y2 = (int)(Position.Y + _halfWidth * Math.Cos(AngleToDegree(Rotation)) - _halfHeight * Math.Sin(AngleToDegree(Rotation)));

            //for (int x = Math.Min(x1, x2); x < Math.Max(x1, x2); x++)
            //{
            //    for (int y = Math.Min(y1, y2); y < Math.Max(y1, y2); y++)
            //    {
            //        if (x < 0 || y < 0 || x >= image.Width || y >= image.Height)
            //        {
            //            continue;
            //        }
            //        yield return new Point(x, y);
            //    }
            //}
            double angle = Rotation * Math.PI / 180;
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            int halfWidth = Size.Width / 2;
            int halfHeight = Size.Height / 2;
            int X0 = Position.X;
            int Y0 = Position.Y;
            int[] x_values = new int[4];
            int[] y_values = new int[4];
            x_values[0] = (int)(X0 + halfWidth * cos - halfHeight * sin);
            y_values[0] = (int)(Y0 + halfWidth * sin + halfHeight * cos);
            x_values[1] = (int)(X0 + halfWidth * cos + halfHeight * sin);
            y_values[1] = (int)(Y0 + halfWidth * sin - halfHeight * cos);
            x_values[2] = (int)(X0 - halfWidth * cos - halfHeight * sin);
            y_values[2] = (int)(Y0 - halfWidth * sin + halfHeight * cos);
            x_values[3] = (int)(X0 - halfWidth * cos + halfHeight * sin);
            y_values[3] = (int)(Y0 - halfWidth * sin - halfHeight * cos);
            int left = x_values.Min();
            int right = x_values.Max();
            int top = y_values.Min();
            int bottom = y_values.Max();
            for (int x = left; x < right; x++)
            {
                for (int y = top; y < bottom; y++)
                {
                    if (x < 0 || y < 0 || x >= image.Width || y >= image.Height)
                    {
                        continue;
                    }
                    yield return new Point(x, y);
                }
            }
        }

        public static double AngleToDegree(float degree)
        {
            return (Math.PI / 180) * degree;
        }
    }
}
