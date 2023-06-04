using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            for (int x = Position.X; x < Position.X + Size.Width; x++)
            {
                for (int y = Position.Y; y < Position.Y + Size.Height; y++)
                {
                    if (x < 0 || y < 0 || x >= image.Width || y >= image.Height)
                    {
                        continue;
                    }
                    yield return new Point(x, y);
                }
            }
        }
    }
}
