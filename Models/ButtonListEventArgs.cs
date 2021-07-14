using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Q.Models
{
    public class ButtonListEventArgs : EventArgs
    {
        public List<Button> ButtonList { get; set; }
    }
}