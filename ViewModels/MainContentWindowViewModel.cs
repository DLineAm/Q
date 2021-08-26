using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Q.Models;
using Q.Services;
using Q.Views;
using QCore;
using QMappingServices;
using static Q.App;
using TaskBarIcon = QCore.TaskBarIcon;

namespace Q.ViewModels
{
    public class MainContentWindowViewModel : BindableBase, IBaseViewModel
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

        private bool _taskBarIsHidden;
        public bool TaskBarIsHidden { get => _taskBarIsHidden; set => SetProperty(ref _taskBarIsHidden, value); }

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
            Sketches = ToSketchesList(WIW.GetListOfWindowSketches(type).ToList()).Cast<WindowSketch>().ToList();
        }

        private void WIWOnChangeListEvent(IList<object> sketches)
        {
            Sketches = sketches.Cast<WindowSketch>().ToList();
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

        public string SketchType { get; set; }

        public List<object> ToSketchesList(List<KeyValuePair<object, IServiceBaseContainer>> pairs)
        {
            var result = (from items in pairs
                select new WindowSketch
                {
                    Name = items.Value.Title,
                    Type = items.Key.GetType(),
                    Sketch = new VisualBrush((UIElement) items.Value),
                    ContentSketch = new VisualBrush((UIElement) items.Key)
                });
            return result.Cast<object>().ToList();
            //var result = (from items in _windowMapping
            //    where items.Key.GetType() == type
            //    select new WindowSketch
            //    {
            //        Name = ((ContentWindowViewModel)items.Value.DataContext).Title,
            //        Type = type,
            //        Sketch = new VisualBrush(items.Value),
            //        ContentSketch = new VisualBrush(items.Key)
            //    }).ToList();
        }

        public void SetSketches(List<object> sketchesList)
        {
            Sketches = sketchesList.Cast<WindowSketch>().ToList();
        }
    }
}
