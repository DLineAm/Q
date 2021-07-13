using System;
using System.Windows.Shapes;

namespace Q.Models
{
    public class PathWithExtensions
    {
        public Path Path { get; set; }

        public string Name { get; set; }

        public PathWithExtensions Clone()
        {
            var result = new PathWithExtensions();
            result.Name = new string(Name.ToCharArray());
            result.Path = new Path{Data = Path.Data};
            return result;
        }
    }
}