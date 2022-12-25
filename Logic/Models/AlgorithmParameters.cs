using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Logic.Models
{
    public class AlgorithmParameters
    {
        [Required]
        public int MaxIterations { get; set; }
        public Color BackgroundColor { get; set; }
        public IEnumerable<Shape>? Shapes { get; set; }
        public int?[]? ShapeSizeLimits { get; set; }
        public int MinShapeSize { get { return ShapeSizeLimits?[0] ?? 1; } }
        public int MaxShapeSize { get { return ShapeSizeLimits?[1] ?? -1; } }
        [Required]
        public bool UseColorDict { get; set; }
        [Required]
        public bool AllowRotation { get; set; }
        [Required]
        public bool AllowAlpha { get; set; }
    }

    public enum Shape
    {
        Ellipse,
        Rectangle,
        Traingle
    }
}
