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
        public int MaxIterations { get; set; }

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

        public IEnumerable<Type> Shapes { get; set; } = new List<Type>();

        public (double Min, double Max) ShapeSizeLimits { get; set; }
        public int MinShapeSize { get { return (int)ShapeSizeLimits.Min; } }
        public int MaxShapeSize { get { return (int)ShapeSizeLimits.Max; } }

        public bool AllowRotation { get; set; }

        public bool AllowAlpha { get; set; }

        public double UseBackgroundColorChance { get; set; }

        public ColorDictParameters ColorDictParameters { get; set; }

        public int ImagePresentationInterval { get; set; }
    }

    public class ColorDictParameters
    {
        public bool Enabled { get; set; }

        public double ResolutionDouble { get; set; }
        public int Resolution { get { return (int)ResolutionDouble; } }

        public int StartUsingFromIteration { get; set; }

        public double UseColorDictChance { get; set; }
    }
}
