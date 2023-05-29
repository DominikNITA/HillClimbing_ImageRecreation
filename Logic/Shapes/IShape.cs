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
        string DisplayName { get; set; }

        void Draw(Brush brush, Size shapeSize, Point shapePosition);


    }
}
