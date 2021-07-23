using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using Q.Models;
using Q.Services;
using Q.Views;
using QMappingServices;
using static Q.App;

namespace Q.ViewModels
{
    public class MainContentWindowViewModel : BindableBase
    {
        public MainContentWindowViewModel(bool isDebug)
        {
            this.Initialise();
            //App.WIW.ChangeListEvent += WIWOnChangeListEvent;
            //App.IMS.IconsListUpdateEvent += IMSOnIconsListUpdateEvent;
            //Sketches = WIW.GetListOfWindowSketches<LoginControl>();
            SketchType = nameof(LoginControl);
            if (isDebug) { }

            Instance = this;
        }

        private void IMSOnIconsListUpdateEvent(IList<object> list)
        {
            Icons = list.Cast<TaskBarIcon>().ToList();
        }

        private void Initialise()
        {
            //    var a = Assembly.LoadFrom(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\" + "QCore\\bin\\Debug\\netcoreapp3.1\\QCore.dll")));
            //    var classType = a.GetType("QCore.EventStorage");
            //    var wiwEvent = classType.GetEvent("ChangeListEvent");
            //    var wiwMethod = this.GetType().GetMethod("WIWOnChangeListEvent");
            //    var wiwDelegate = Delegate.CreateDelegate(wiwEvent.EventHandlerType, wiwMethod);
            //    wiwEvent.AddEventHandler(this, wiwDelegate);

            //    var imsEvent = classType.GetEvent("IconsListUpdateEvent", BindingFlags.Public | BindingFlags.Static);
            //    var imsMethod = this.GetType().GetMethod("IMSOnIconsListUpdateEvent");
            //    var imsDelegate = Delegate.CreateDelegate(wiwEvent.EventHandlerType, wiwMethod);
            //    imsEvent.AddEventHandler(this, wiwDelegate);

            EventStorage.ChangeListEvent += WIWOnChangeListEvent;
            EventStorage.IconsListUpdateEvent += IMSOnIconsListUpdateEvent;
        }

        private void WIWOnChangeListEvent(Type type)
        {
            Sketches = WIW.GetListOfWindowSketches(type).Cast<WindowSketch>().ToList();
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

        private List<TaskBarIcon> _icons;
        public List<TaskBarIcon> Icons
        {
            get => _icons;
            set => SetProperty(ref _icons, value);
        }

        public string SketchType;
    }
}
