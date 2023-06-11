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

        protected record BoundingBox(int Left, int Right, int Top, int Bottom);

        protected BoundingBox GetBoundingBoxIgnoringRotation()
        {
            return new(Position.X - _halfWidth,
                Position.X + _halfWidth,
                Position.Y - _halfHeight,
                Position.Y + _halfHeight);
        }

        protected BoundingBox GetBoundingBoxWithRotation()
        {
            double angle = Rotation * Math.PI / 180;
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            int X0 = Position.X;
            int Y0 = Position.Y;

            int[] x_values = new int[4];
            int[] y_values = new int[4];
            x_values[0] = (int)(X0 + _halfWidth * cos - _halfHeight * sin);
            y_values[0] = (int)(Y0 + _halfWidth * sin + _halfHeight * cos);
            x_values[1] = (int)(X0 + _halfWidth * cos + _halfHeight * sin);
            y_values[1] = (int)(Y0 + _halfWidth * sin - _halfHeight * cos);
            x_values[2] = (int)(X0 - _halfWidth * cos - _halfHeight * sin);
            y_values[2] = (int)(Y0 - _halfWidth * sin + _halfHeight * cos);
            x_values[3] = (int)(X0 - _halfWidth * cos + _halfHeight * sin);
            y_values[3] = (int)(Y0 - _halfWidth * sin - _halfHeight * cos);

            int left = x_values.Min() - 1;
            int right = x_values.Max() + 1;
            int top = y_values.Min() - 1;
            int bottom = y_values.Max() + 1;

            return new(left, right, top, bottom);
        }

        protected IEnumerable<Point> GetModifiedPixelsFromBoundingBox(BoundingBox boundingBox, Bitmap image)
        {
            for (int x = boundingBox.Left; x < boundingBox.Right; x++)
            {
                for (int y = boundingBox.Top; y < boundingBox.Bottom; y++)
                {
                    if (x < 0 || y < 0 || x >= image.Width || y >= image.Height)
                    {
                        continue;
                    }
                    yield return new Point(x, y);
                }
            }
        }

        public virtual IEnumerable<Point> GetModifiedPixels(Bitmap image)
        {
            BoundingBox boundingBox;
            if (Rotation == 0)
            {
                boundingBox = GetBoundingBoxIgnoringRotation();
            }
            else
            {
                boundingBox = GetBoundingBoxWithRotation();
            }
            return GetModifiedPixelsFromBoundingBox(boundingBox, image);
        }
    }
}
