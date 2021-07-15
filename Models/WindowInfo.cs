using System;
using System.Windows;
using System.Windows.Controls;
using Google.Protobuf.Reflection;

namespace Q.Models
{
    public class WindowInfo
    {
        //public string TypeName { get; set; }
        public string ContentName { get; set; }
        public string AssemblyName { get; set; }

        public string IconName { get; set; }

        public string Title { get; set; }

        public double CanvasX { get; set; }
        public double CanvasY { get; set; }
        public double CanvasZ { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        public WindowState State { get; set; }

        public string Parent { get; set; }
    }
}