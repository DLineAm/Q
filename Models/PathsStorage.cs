using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using GPath = System.Windows.Shapes.Path;

namespace Q.Models
{
    public static class PathsStorage
    {
        private static readonly List<PathWithExtensions> Paths;

        static PathsStorage()
        {
            Paths = new List<PathWithExtensions>();
            //var path = new GPath
            //{
            //    Data = Geometry.Parse(
            //        "m 9.8913855 79.468164 c -1.7733279 -1.773328 -0.249742 -4.392381 4.2259415 -7.26442 2.690935 -1.726768 5.165025 -4.136178 5.497978 -5.354246 0.332953 -1.218067 0.08168 -5.398909 -0.558389 -9.290761 -0.839347 -5.103554 -0.868014 -8.241231 -0.102838 -11.255791 0.583506 -2.298832 0.714296 -4.393921 0.290644 -4.655753 -0.423652 -0.261831 -2.891836 0.246832 -5.484854 1.130362 -4.2897335 1.461659 -4.8564505 1.464546 -6.2889614 0.03204 -2.19827 -2.19827 -0.387558 -4.134923 5.7819494 -6.184105 4.788895 -1.590615 5.001977 -1.570463 12.173039 1.151238 6.560341 2.489903 8.563554 2.770187 19.798828 2.770187 11.23527 0 13.23848 -0.280284 19.79882 -2.770192 7.17106 -2.721701 7.38414 -2.741853 12.17304 -1.151238 6.16951 2.049182 7.98022 3.985835 5.78195 6.184105 -1.43251 1.432511 -1.99923 1.429624 -6.28896 -0.03204 -2.59302 -0.88353 -5.06121 -1.392193 -5.48486 -1.130362 -0.42365 0.261832 -0.29286 2.356921 0.29065 4.655753 0.76517 3.01456 0.73651 6.152237 -0.10284 11.255791 -0.64007 3.891852 -0.89134 8.072694 -0.55839 9.290761 0.33295 1.218068 2.83024 3.642365 5.54953 5.387329 3.71246 2.382279 4.86904 3.69643 4.64261 5.275117 -0.16585 1.15635 -1.07613 2.25006 -2.02284 2.430466 -1.72346 0.328424 -10.36029 -4.985008 -12.32054 -7.579674 -0.84464 -1.117991 -1.77647 -0.7061 -4.92313 2.176132 -4.08774 3.744219 -9.64032 6.153277 -12.05068 5.228335 -1.14372 -0.438884 -1.48882 -3.739178 -1.70876 -16.341163 l -0.2756 -15.791204 h -2.5 -2.5 l -0.27561 15.791209 c -0.21994 12.601985 -0.56504 15.902279 -1.70876 16.341163 -2.410354 0.924942 -7.962941 -1.484116 -12.050676 -5.228335 -3.146665 -2.882232 -4.078497 -3.294123 -4.923131 -2.176132 -1.578293 2.089092 -10.210427 7.772095 -11.805336 7.772095 -0.771737 0 -1.703158 -0.3 -2.0698245 -0.666667 z M 30.724719 33.799692 c -2.75 -0.711318 -6.125 -1.914757 -7.5 -2.674309 -1.375 -0.759552 -4.3 -1.630652 -6.5 -1.935778 -3.45206 -0.478778 -4 -0.897236 -4 -3.054774 0 -2.274663 0.430079 -2.527551 4.771499 -2.805669 3.622604 -0.23207 5.400751 0.1893 7.384858 1.75 3.217839 2.531152 4.195797 2.528516 7.256244 -0.01956 l 2.412601 -2.00869 -2.168312 -1.518745 c -2.715902 -1.902292 -5.478288 -7.117403 -4.760629 -8.987594 0.954066 -2.486255 4.46047 -1.587475 6.088409 1.560613 2.145899 4.149711 4.535588 5.225727 8.381604 3.77402 2.4121 -0.910468 3.85535 -0.910468 6.26746 0 3.84601 1.451707 6.2357 0.375691 8.3816 -3.77402 1.62793 -3.148088 5.13434 -4.046868 6.0884 -1.560613 0.66581 1.735069 -1.68336 6.447352 -4.47244 8.97144 l -2.07347 1.876462 2.37882 1.871177 c 3.00081 2.360438 3.69523 2.34189 6.907 -0.184491 1.98411 -1.5607 3.76225 -1.98207 7.38486 -1.75 4.34142 0.278119 4.7715 0.531007 4.7715 2.80567 0 2.148612 -0.56223 2.585504 -4 3.108328 -2.2 0.33458 -6.25 1.644302 -9 2.910494 -4.15438 1.912814 -7.02949 2.356047 -17 2.620755 -7.59697 0.201692 -13.8346 -0.15595 -17.000004 -0.974716 z"),
            //    Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
            //};

            //Paths.Add(new PathWithExtensions{ Name = "Bug", Path = path});

            //GetXamlViewBox("Bug.xaml");
            //GetXamlViewBox("Store.xaml");

            //var stream = File.ReadAllText(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\" + "Resources\\Store.xaml")));

            //var vb = (Viewbox)XamlReader.Parse(stream);

           
            Paths.Add(new PathWithExtensions{Name = "Store", ViewBox = GetXamlViewBox("Store.xaml")});
            Paths.Add(new PathWithExtensions{Name = "Bug", ViewBox = GetXamlViewBox("Bug.xaml")});

            //XamlReader.GetWpfSchemaContext();
        }

        public static string GetIconName(Viewbox vb)
        {
            try
            {
                var result = Paths.First(p => p.ViewBox.Name == vb.Name);
                return result.Name;
            }
            catch
            {
                return "";
            }
        }

        private static Viewbox GetXamlViewBox(string path)
        {
            try
            {
                var stream = File.ReadAllText(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\" + "Resources\\" + path)));

                var vb = (Viewbox)XamlReader.Parse(stream);

                return vb;
            }
            catch
            {
                return new Viewbox();
            }
        }

        public static List<PathWithExtensions> GetPath(string name)
        {
            return Paths.FindAll(p => p.Name.Contains(name)).Select(Clone).ToList();
        }

        private static PathWithExtensions Clone(PathWithExtensions parent)
        {
            if (parent.Path != null)
            {
                var pathXaml = XamlWriter.Save(parent.Path);
                var stringReader = new StringReader(pathXaml);
                var xmlReader = XmlReader.Create(stringReader);
                var newPath = (GPath)XamlReader.Load(xmlReader);
                return new PathWithExtensions {Name = new string(parent.Name.ToCharArray()), Path = newPath};
            }
            else
            {
                var vbXaml = XamlWriter.Save(parent.ViewBox);
                var stringReader = new StringReader(vbXaml);
                var xmlReader = XmlReader.Create(stringReader);
                var newvb = (Viewbox)XamlReader.Load(xmlReader);
                return new PathWithExtensions {Name = new string(parent.Name.ToCharArray()), ViewBox = newvb};
            }
        }
    }
}