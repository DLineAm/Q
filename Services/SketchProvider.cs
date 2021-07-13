using System.Collections.Generic;
using System.Windows.Documents;
using Q.Models;
using Q.ViewModels;

namespace Q.Services
{
    public static class SketchProvider
    {
        public static List<WindowSketch> GetSketches()
        {
            return MainContentWindowViewModel.Instance.Sketches;
        }

        public static int GetIndexOfSketchList(WindowSketch sketch)
        {
            return MainContentWindowViewModel.Instance.Sketches.IndexOf(sketch);
        }
    }
}