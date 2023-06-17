using Logic.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.ScoreCalculator
{
    public interface IScoreCalculator
    {
        double CompareScores(Bitmap currentIterationBitmap, Bitmap lastImageBitmap, IShape shape, Bitmap targetImageBitmap);
        double GetScoreForWholeImage(Bitmap image, Bitmap targetImageBitmap);
    }
}
