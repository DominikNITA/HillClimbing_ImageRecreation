using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Shapes
{
    public interface IComparable
    {
        static abstract IEnumerable<Point> GetModifiedPixels(Size shapeSize, Point shapePosition, float shapeRotation);
    }
}
