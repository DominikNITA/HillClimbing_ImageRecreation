using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static Logic.Helpers.ValidationHelper;
using Logic.Shapes;

namespace Logic.Models
{
    public class AlgorithmParameters
    {
        [Required]
        public int MaxIterations { get; set; }
        
        [Required]
        [StringLength(9)]
        public string BackgroundBaseColorString { get; set; } = string.Empty;
        public Color BackgroundColor
        {
            get
            {
                return Color.FromArgb(
                    int.Parse(BackgroundBaseColorString.Substring(7, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(BackgroundBaseColorString.Substring(1, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(BackgroundBaseColorString.Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(BackgroundBaseColorString.Substring(5, 2), System.Globalization.NumberStyles.HexNumber)
                    );
            }
        }
        [Required]
        [MustHaveOneElement("Select at least one shape")]
        public IEnumerable<Type>? Shapes { get; set; }
        [Required]
        public (double, double) ShapeSizeLimits { get; set; }
        public int MinShapeSize { get { return (int)ShapeSizeLimits.Item1; } }
        public int MaxShapeSize { get { return (int)ShapeSizeLimits.Item2; } }
        //TODO: Add parameters for bezier curve
        [Required]
        public bool AllowRotation { get; set; }
        [Required]
        public bool AllowAlpha { get; set; }
        [Range(0,1)]
        public double UseBackgroundColorChance { get; set; }
        [Required]
        public bool UseColorDict { get; set; }

        /// <summary>
        /// Every how many iterations the generated image should be saved and sent back to user
        /// </summary>
        public int ImagePresentationInterval { get; set; }
    }
}
