using System;

namespace Q.Services
{
    public struct SketchIcon<TC> : ISketchIcon<TC>
    {
        public string Name { get; set; }
        public Action ClickAction { get; set; }
    }
}