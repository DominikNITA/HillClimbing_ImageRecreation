using Logic.ScoreCalculator;
using Logic.Shapes;
using Logic.Tests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace UnitTests
{
    public class BasicScoreCalculatorTests
    {
        private readonly ITestOutputHelper _output;
        public BasicScoreCalculatorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        const int WIDTH = 10, HEIGHT = 10;

        [Fact]
        public void GetScoreForWholeImage_SameColor()
        {
            GetScoreForWholeImage_BaseMethod(Color.Pink, Color.Pink, 0);
        }

        [Fact]
        public void GetScoreForWholeImage_WhiteToBlack()
        {
            GetScoreForWholeImage_BaseMethod(Color.White, Color.Black, WIDTH * HEIGHT * 3);
        }

        [Fact]
        public void GetScoreForWholeImage_CompareToTransparent()
        {
            GetScoreForWholeImage_BaseMethod(Color.White, Color.Transparent, WIDTH * HEIGHT);
        }

        [Fact]
        public void GetScoreForWholeImage_TransparentToWhite()
        {
            GetScoreForWholeImage_BaseMethod(Color.Transparent, Color.White, WIDTH * HEIGHT * 4);
        }

        [Fact]
        public void GetScoreForWholeImage_OnePixelBlackAndNotFullyTransparent()
        {
            var scoreCalculator = new BasicScoreCalculator();

            Bitmap testImageBitmap = new(WIDTH, HEIGHT, PixelFormat.Format32bppArgb);
            Graphics testImageGraphics = Graphics.FromImage(testImageBitmap);
            testImageGraphics.FillRectangle(new SolidBrush(Color.Transparent), new System.Drawing.Rectangle(0, 0, WIDTH, HEIGHT));
            // One pixel is black instead of being transparent
            testImageGraphics.FillRectangle(new SolidBrush(Color.Black), new System.Drawing.Rectangle(0, 0, 1, 1));

            Bitmap targetImageBitmap = new(WIDTH, HEIGHT, PixelFormat.Format32bppArgb);
            Graphics targetImageGraphics = Graphics.FromImage(targetImageBitmap);
            targetImageGraphics.FillRectangle(new SolidBrush(Color.Transparent), new System.Drawing.Rectangle(0, 0, WIDTH, HEIGHT));

            var score = scoreCalculator.GetScoreForWholeImage(testImageBitmap, targetImageBitmap);

            testImageGraphics.Dispose();
            targetImageGraphics.Dispose();

            // In this case only the alpha component is important
            Assert.Equal(1, score);
        }

        [Fact]
        public void GetScoreForWholeImage_OnePixelWhiteAndNotPartiallyTransparent()
        {
            var scoreCalculator = new BasicScoreCalculator();

            Bitmap testImageBitmap = new(WIDTH, HEIGHT, PixelFormat.Format32bppArgb);
            Graphics testImageGraphics = Graphics.FromImage(testImageBitmap);
            testImageGraphics.FillRectangle(new SolidBrush(Color.FromArgb(10, Color.Black)), new System.Drawing.Rectangle(0, 0, WIDTH, HEIGHT));
            // One pixel is white instead of being partially transparent
            testImageGraphics.FillRectangle(new SolidBrush(Color.White), new System.Drawing.Rectangle(0, 0, 1, 1));

            Bitmap targetImageBitmap = new(WIDTH, HEIGHT, PixelFormat.Format32bppArgb);
            Graphics targetImageGraphics = Graphics.FromImage(targetImageBitmap);
            targetImageGraphics.FillRectangle(new SolidBrush(Color.FromArgb(10, Color.Black)), new System.Drawing.Rectangle(0, 0, WIDTH, HEIGHT));

            var score = scoreCalculator.GetScoreForWholeImage(testImageBitmap, targetImageBitmap);

            testImageGraphics.Dispose();
            targetImageGraphics.Dispose();

            _output.WriteLine(score.ToString());
            Assert.InRange(score, 3 ,4);
        }

        private void GetScoreForWholeImage_BaseMethod(Color testImageColor, Color targetImageColor, double expectedScore)
        {
            var scoreCalculator = new BasicScoreCalculator();

            Bitmap testImageBitmap = new(WIDTH, HEIGHT, PixelFormat.Format32bppArgb);
            Graphics testImageGraphics = Graphics.FromImage(testImageBitmap);
            testImageGraphics.FillRectangle(new SolidBrush(testImageColor), new System.Drawing.Rectangle(0, 0, WIDTH, HEIGHT));

            Bitmap targetImageBitmap = new(WIDTH, HEIGHT, PixelFormat.Format32bppArgb);
            Graphics targetImageGraphics = Graphics.FromImage(targetImageBitmap);
            targetImageGraphics.FillRectangle(new SolidBrush(targetImageColor), new System.Drawing.Rectangle(0, 0, WIDTH, HEIGHT));

            var score = scoreCalculator.GetScoreForWholeImage(testImageBitmap, targetImageBitmap);

            testImageGraphics.Dispose();
            targetImageGraphics.Dispose();

            Assert.Equal(expectedScore, score);
        }
    }
}
