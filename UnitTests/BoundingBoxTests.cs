using Logic.Shapes;
using Logic.Tests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class BoundingBoxTests
    {
        private class BarTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var shapeTypes = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(s => s.GetTypes())
                                    .Where(p => typeof(Shape).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

                foreach (var shapeType in shapeTypes)
                {
                    for (int i = 0; i < 360; i++)
                    {
                        yield return new object[] { i, shapeType };
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(BarTestData))]
        public void BarTest(int rotationAngle, Type shapeType)
        {
            var shapeTester = new ShapeTester();
            Bitmap image;
            Graphics initialGraphics;

            shapeTester.CreateTestImage(
                shapeType,
                new Size() { Width = 40, Height = 22 },
                new Point() { X = 30, Y = 55 },
                rotationAngle,
                false,
                out image,
                out initialGraphics
                );

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    if (image.GetPixel(x, y).ToArgb() == ShapeTester.SHAPE_COLOR.ToArgb())
                    {
                        Assert.Fail($"Failed at: ({x}, {y})");
                    }
                }
            }

            initialGraphics.Dispose();
        }
    }
}
