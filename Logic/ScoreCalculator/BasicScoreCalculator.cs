using Logic.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ScoreCalculator
{
    public class BasicScoreCalculator : IScoreCalculator
    {
        public double CompareScores(Bitmap firstBitmap, Bitmap secondBitmap, IShape shape, Bitmap targetImageBitmap)
        {
            double score = 0;
            foreach (var pixel in shape.GetModifiedPixels(firstBitmap))
            {
                var firstBitmapPixel = firstBitmap.GetPixel(pixel.X, pixel.Y);
                var secondBitmapPixel = secondBitmap.GetPixel(pixel.X, pixel.Y);
                score += CalculateScoreForPixel(pixel.X, pixel.Y, firstBitmapPixel, targetImageBitmap) - CalculateScoreForPixel(pixel.X, pixel.Y, secondBitmapPixel, targetImageBitmap);
            }
            return score;
        }

        public double GetScoreForWholeImage(Bitmap image, Bitmap targetImageBitmap)
        {
            double score = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    score += CalculateScoreForPixel(x, y, image.GetPixel(x, y), targetImageBitmap);
                }
            }
            return score;
        }

        /// <summary>
        /// Calculates the score for given pixel. Best score 0, worst score 4
        /// </summary>
        private double CalculateScoreForPixel(int x, int y, Color pixelColor, Bitmap targetImageBitmap)
        {
            var targetPixel = targetImageBitmap.GetPixel(x, y);
            double score = 0;

            if (targetPixel.A == 0)
            {
                // If both pixels are completely transparent we don't compare other colors
                if (pixelColor.A == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Abs((targetPixel.A - pixelColor.A) / (double)255);
                }
            }
            score += Math.Abs((targetPixel.A - pixelColor.A) / (double)255);
            score += Math.Abs((targetPixel.R - pixelColor.R) / (double)255);
            score += Math.Abs((targetPixel.G - pixelColor.G) / (double)255);
            score += Math.Abs((targetPixel.B - pixelColor.B) / (double)255);

            return score;
        }
    }
}
