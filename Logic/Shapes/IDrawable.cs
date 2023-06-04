using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Shapes
{
    public interface IDrawable
    {
        static abstract void Draw(Graphics graphics, Brush brush, Size shapeSize, Point shapePosition, float shapeRotation);
    }
}
