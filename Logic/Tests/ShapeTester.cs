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
    public static class ShapeTester
    {
        private const int IMAGE_WIDTH = 100, IMAGE_HEIGHT = 100;
        public static Color BACKGROUND_COLOR = Color.LightGray;
        public static Color SHAPE_COLOR = Color.DarkBlue;
        public static Color BOUNDING_BOX_COLOR = Color.FromArgb(50, Color.Red);
        public static string CreateBase64Image(Type shapeType, Size size, Point position, double rotation, bool isUsingBackgroundColor)
        {
            Bitmap image;
            Graphics initialGraphics;

            CreateTestImage(shapeType, size, position, rotation, isUsingBackgroundColor, out image, out initialGraphics);

            string result;
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                result = Convert.ToBase64String(ms.GetBuffer()); //Get Base64
            }

            initialGraphics.Dispose();
            image.Dispose();

            return result;
        }

        public static void CreateTestImage(Type shapeType, Size size, Point position, double rotation, bool isUsingBackgroundColor, out Bitmap image, out Graphics initialGraphics)
        {
            image = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT, PixelFormat.Format32bppArgb);
            initialGraphics = Graphics.FromImage(image);
            var shapeBrush = new SolidBrush(BACKGROUND_COLOR);
            initialGraphics.FillRectangle(shapeBrush, new System.Drawing.Rectangle(0, 0, IMAGE_WIDTH, IMAGE_HEIGHT));

            var shapeToDraw = ShapeFactory.CreateShape(shapeType, SHAPE_COLOR, size, position, (float)rotation, isUsingBackgroundColor);
            shapeToDraw.Draw(initialGraphics);

            var outlineBrush = new SolidBrush(BOUNDING_BOX_COLOR);
            foreach (var pixel in shapeToDraw.GetModifiedPixels(image))
            {
                initialGraphics.FillRectangle(outlineBrush, pixel.X, pixel.Y, 1, 1);
            }

            shapeBrush.Dispose();
            outlineBrush.Dispose();
        }
    }

    public class ShapeTestResult
    {
        public int MyProperty { get; set; }
    }
}
