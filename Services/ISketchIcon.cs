using System;
using System.Diagnostics;

namespace Q.Services
{
    public interface ISketchIcon<out TC>
    {
        public string Name { get; set; }

        public Type VmType => typeof(TC);

        public Action ClickAction { get; set; }

        public ISketchIcon<T> ToType<T>()
        {
            try
            {
                var result = (ISketchIcon<T>)this;

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}