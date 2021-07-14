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
        public MainContentWindowViewModel(bool isDebug)
        {
            WIW.ChangeListEvent += WIWOnChangeListEvent;
            //Sketches = WIW.GetListOfWindowSketches<LoginControl>();
            SketchType = nameof(LoginControl);
            if (isDebug){}

            Instance = this;
        }

        private void WIWOnChangeListEvent(Type type)
        {
            Sketches = WIW.GetListOfWindowSketches(type);
        }

        public static MainContentWindowViewModel Instance { get; private set; }

        public void Subscribe()
        {
            //FormInitializeNotificator.Subscribe(() =>
            //{
            //    ISketchIcon<LoginControl> icon = new SketchIcon<LoginControl> { Name = "Тестирование" };
            //    IMS.SetActionClick(icon);
            //    IMS.TryAddIcon(icon, "Bug");
            //});
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

        public string SketchType;
    }
}
