using Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal class ColorBucketManager
    {
        private readonly int _bucketSize;
        private IReadOnlyList<ColorBucket> _buckets;
        Random _random;


        public ColorBucketManager(int bucketSize, Bitmap image)
        {
            _bucketSize = bucketSize;
            _random = new Random();
            SeedColorBucketsFromImage(image);
        }

        private void SeedColorBucketsFromImage(Bitmap image)
        {
            int[,,] _colorBuckets;
            var bucketNumber = (int)Math.Ceiling(255 / (double)_bucketSize);
            _colorBuckets = new int[bucketNumber, bucketNumber, bucketNumber];

            double visiblePixelCount = 0;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    var r = pixel.R / _bucketSize;
                    var g = pixel.G / _bucketSize;
                    var b = pixel.B / _bucketSize;
                    if (pixel.A != 0)
                    {
                        _colorBuckets[r, g, b]++;
                        visiblePixelCount++;
                    }
                }
            }

            var tempBuckets = new List<ColorBucket>();
            for (int r = 0; r < bucketNumber; r++)
            {
                for (int g = 0; g < bucketNumber; g++)
                {
                    for (int b = 0; b < bucketNumber; b++)
                    {
                        var bucket = new ColorBucket()
                        {
                            R = r * _bucketSize + _bucketSize / 2,
                            G = g * _bucketSize + _bucketSize / 2,
                            B = b * _bucketSize + _bucketSize / 2,
                            Percent = _colorBuckets[r, g, b] / visiblePixelCount,
                        };
                        tempBuckets.Add(bucket);
                    }
                }
            }

            _buckets = tempBuckets.OrderByDescending(b => b.Percent).ToList();
            Console.WriteLine("Most common color in bucket:");
            Console.WriteLine(_buckets.FirstOrDefault()?.PropertiesToString());
        }

        public Color GetRandomColor(double minimalBucketPercentage)
        {
            //1. Get only buckets that have at least minimalBucketPercentage
            var numberOfBucketsToTakeFrom = _buckets.Count;
            for (int i = 0; i < _buckets.Count; i++)
            {
                if (_buckets[i].Percent < minimalBucketPercentage)
                {
                    numberOfBucketsToTakeFrom = i + 1;
                    break;
                }
            }
            var goodBuckets = _buckets.Take(numberOfBucketsToTakeFrom).ToList();

            //2. Get random bucket
            var randomBucket = goodBuckets[_random.Next(0, goodBuckets.Count)];

            //3. Get random color from bucket
            var r = _random.Next(randomBucket.R - _bucketSize / 2, randomBucket.R + _bucketSize / 2);
            var g = _random.Next(randomBucket.G - _bucketSize / 2, randomBucket.G + _bucketSize / 2);
            var b = _random.Next(randomBucket.B - _bucketSize / 2, randomBucket.B + _bucketSize / 2);

            return Color.FromArgb(
                Math.Clamp(r, 0, 255),
                Math.Clamp(g, 0, 255),
                Math.Clamp(b, 0, 255)
                );
        }
    }

    internal class ColorBucket
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public double Percent { get; set; }
    }
}
