using System;
using System.Windows.Media;

namespace Q.Models
{
    public class WindowSketch
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public VisualBrush Sketch { get; set; }
    }
}