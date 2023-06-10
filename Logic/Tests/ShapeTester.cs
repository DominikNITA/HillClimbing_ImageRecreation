using Logic.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Tests
{
    public class ShapeTester
    {
        private const int IMAGE_WIDTH = 100, IMAGE_HEIGHT = 100;
        private static Color BACKGROUND_COLOR = Color.LightGray;
        private static Color SHAPE_COLOR = Color.DarkBlue;
        public string CreateImage(Type shapeType, Color color, Size size, Point position, float rotation, bool isUsingBackgroundColor)
        {
            var image = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT, PixelFormat.Format32bppArgb);

            Graphics initialGraphics = Graphics.FromImage(image);
            initialGraphics.FillRectangle(new SolidBrush(BACKGROUND_COLOR), new System.Drawing.Rectangle(0, 0, IMAGE_WIDTH, IMAGE_HEIGHT));

            var shapeToDraw = ShapeFactory.CreateShape(shapeType, SHAPE_COLOR, size, position, rotation, isUsingBackgroundColor);
            shapeToDraw.Draw(initialGraphics);

            var outineBrush = new SolidBrush(Color.FromArgb(50, Color.Red));
            foreach (var pixel in shapeToDraw.GetModifiedPixels(image))
            {
                initialGraphics.FillRectangle(outineBrush, pixel.X, pixel.Y, 1, 1);
            }

            string result;
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                result = Convert.ToBase64String(ms.GetBuffer()); //Get Base64
            }

            initialGraphics.Dispose();
            image.Dispose();
            outineBrush.Dispose();

            return result;
        }
    }

    public class ShapeTestResult
    {
        public int MyProperty { get; set; }
    }
}
