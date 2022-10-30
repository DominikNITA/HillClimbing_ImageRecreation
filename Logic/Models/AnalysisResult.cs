using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class AnalysisResult
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool ContainsAlpha { get; set; }
        public int ColorCount { get; set; }
        public int PixelCount { get { return Width * Height; } }
    }
}
