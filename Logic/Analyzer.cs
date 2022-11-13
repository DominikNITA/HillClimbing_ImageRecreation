using Logic.Models;
using System.Drawing;

namespace Logic
{
    public class Analyzer
    {
        public static AnalysisResult AnalyzeImage(string path)
        {
            Bitmap bitmap = new(path);
            Console.WriteLine($"Analyzing image: {Path.GetFileName(path)}");
            return new()
            {
                Height = bitmap.Height,
                Width = bitmap.Width,
                ColorCount = CountDistinctColors(bitmap),
                ContainsAlpha = true
            };
        }

        private static int CountDistinctColors(Bitmap bitmap)
        {
            HashSet<int> colors = new();
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    colors.Add(pixel.ToArgb());                    
                }
            }
            return colors.Distinct().Count();
        }
    }
}