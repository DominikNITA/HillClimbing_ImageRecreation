using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class AlgorithmParameters
    {
        public int MaxIterations { get; set; }
        public Color BackgroundColor { get; set; }
        public IEnumerable<Shape>? Shapes { get; set; }
        public int MaxShapeSize { get; set; }
        public int MinShapeSize { get; set; }
        public bool UseColorDict { get; set; }
        public bool AllowRotation { get; set; }
        public bool AllowAlpha { get; set; }
    }

    public enum Shape
    {
        Ellipse,
        Rectangle,
        Traingle
    }
}
