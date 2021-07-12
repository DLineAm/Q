using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Q.Models;
using Q.Services;
using Q.Views;

namespace Q.ViewModels
{
    public class MainContentWindowViewModel : BindableBase
    {
        public MainContentWindowViewModel()
        {
            Sketches = WIW.GetListOfWindowSketches<LoginControl>();
        }

        private string _title = App.NameTitle + "Главная форма";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private List<WindowSketch> _sketches;
        public List<WindowSketch> Sketches
        {
            get => _sketches;
            set => SetProperty(ref _sketches, value);
        }
    }
}
