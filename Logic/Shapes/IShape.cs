﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Shapes
{
    public interface IShape
    {
        void Draw(Graphics graphics);

        public IEnumerable<Point> GetModifiedPixels(Bitmap image);
    }
}
