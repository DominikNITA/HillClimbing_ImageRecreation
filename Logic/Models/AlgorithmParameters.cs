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
        public Color BackgroundColor { get; set; }
        public IEnumerable<Shape>? Shapes { get; set; }
        public bool AllowRotation { get; set; }
        public bool UseColorDict { get; set; }
        public int MaxIterations { get; set; }
        public bool ContainsAlpha { get; set; }
    }

    public enum Shape
    {
        Ellipse,
        Rectangle,
        Traingle
    }
}
