using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Newtonsoft.Json;

using Q.Models;
using Q.ViewModels;
using Q.Views;

namespace Q.Services
{
    public static class WIW
    {
        static WIW(){ }

        private static readonly Dictionary<UserControl, ContentWindow> _windowMapping = new Dictionary<UserControl, ContentWindow>();

        public delegate void SketchHandler(Type type);

        public static event SketchHandler ChangeListEvent;

        public static void ShowWindow(MainContentWindow window, UserControl content)
        {
            var wiw = new ContentWindow
            {
                ContentControl = { Content = content },
                BackupHeight = Double.NaN,
                BackupWidth = Double.NaN
            };
            window.WindowPanel.Children.Add(wiw);
            _windowMapping[content] = wiw;
        }

        public static void ShowWindow(UserControl content, double height, double width, string title = "", string iconName = "")
        {
            var instance = MainContentWindow.Instance;
            var wiw = new ContentWindow
            {
                ContentControl = { Content = content },
                Height = height,
                Width = width,
                BackupHeight = height,
                BackupWidth = width
            };

            wiw.Bar.Title = title == "" 
                ? ((UserControl) wiw.ContentControl.Content).GetType().FullName 
                : title;

            instance.NormalPanel.Children.Add(wiw);
            _windowMapping[content] = wiw;

            IMS.FastAddIcon(title, content.GetType(), content.DataContext.GetType(), true, "", true);

            Canvas.SetZIndex(instance.MaximizePanel, 1);

            Canvas.SetZIndex(instance.WindowPanel, 1);

            Canvas.SetZIndex(instance.NormalPanel, 10);

            ChangeTopMost(content);

            if (instance.MaximizePanel.Children.Count == 0) instance.MaximizePanel.Visibility = Visibility.Collapsed;

            ChangeListEvent.Invoke(content.GetType());
        }

        public static void ShowWindow<TVm>(UserControl content, double height, double width, string title = "", string iconName = "") where TVm : new()
        {
           ShowWindow(content, height, width, title, iconName);
           ISketchIcon<object> icon = new SketchIcon<object>()
           {
               Name = title, ClickAction = () =>
               {
                   //var btn = (Button) sender;
                   content.DataContext = Activator.CreateInstance<TVm>();;
                   ShowWindow(content, 400, 600, title);
               }
           };
           IMS.TryAddIcon(icon,
               iconName == "" ? "Bug" : iconName);
        }

        public static void CloseWindow(UserControl content)
        {
            //if(content == null) return;
            if (!_windowMapping.TryGetValue(content, out var value))
                throw new InvalidOperationException("UI for this UserContext is not displayed");

            var parent = VisualTreeHelper.GetParent(value) as Panel;

            parent.Children.Remove(value);

            //window.WindowCanvas.Children.Remove(value);
            _windowMapping.Remove(content);

            var type = content.GetType();

            if (GetListOfWindowSketches(type).Count == 0)
            {
                IMS.ChangeDictValue(type);
            }

            ChangeListEvent.Invoke(content.GetType());

            value = null;
            content = null;
        }

        public static void CloseWindow(int index, Type ucType)
        {
            if (_windowMapping.Count - 1 < index)
                throw new InvalidOperationException("UI for this UserContext is not displayed");

            var list = _windowMapping.Select(p => p.Key)
                .Where(p => p.GetType() == ucType)
                .ToList();

            if(list.Count == -1) return;

            var content = list[index];

            _windowMapping.TryGetValue(list[index], out var value);
            //var content = pair.Key;
            //var value = pair.Value;

            var parent = VisualTreeHelper.GetParent(value) as Panel;

            parent.Children.Remove(value);

            //window.WindowCanvas.Children.Remove(value);
            _windowMapping.Remove(content);

            value = null;
            content = null;

            if (GetListOfWindowSketches(ucType).Count == 0)
            {
                IMS.ChangeDictValue(ucType);
            }

            ChangeListEvent.Invoke(ucType);
        }

        public static void Minimize(UserControl content)
        {
            if (!_windowMapping.TryGetValue(content, out var value))
                throw new InvalidOperationException("UI for this UserContext is not displayed");

            if (value.Visibility == Visibility.Collapsed)
            {
                value.Visibility = Visibility.Visible;
                value.WindowState = WindowState.Normal;
                return;
            }

            value.Visibility = Visibility.Collapsed;
            value.WindowState = WindowState.Minimized;
        }

        public static void ChangeTopMost(UserControl content)
        {
            if (!_windowMapping.TryGetValue(content, out var value))
                throw new InvalidOperationException("UI for this UserContext is not displayed");

            //var parent = VisualTreeHelper.GetParent(value) as Panel;

            //Panel.SetZIndex(value, 10);

            switch (value.WindowState)
            {
                case WindowState.Normal:
                    Panel.SetZIndex(MainContentWindow.Instance.NormalPanel, 10);
                    Panel.SetZIndex(MainContentWindow.Instance.MaximizePanel, 1);
                    Panel.SetZIndex(MainContentWindow.Instance.WindowPanel, 1);
                    MainContentWindow.Instance.WindowPanel.Children.Remove(value);
                    MainContentWindow.Instance.MaximizePanel.Children.Remove(value);
                    MainContentWindow.Instance.NormalPanel.Children.Remove(value);
                    MainContentWindow.Instance.NormalPanel.Children.Add(value);
                    break;
                case WindowState.Minimized:
                    break;
                case WindowState.Maximized:
                    Panel.SetZIndex(MainContentWindow.Instance.NormalPanel, 1);
                    Panel.SetZIndex(MainContentWindow.Instance.MaximizePanel, 10);
                    Panel.SetZIndex(MainContentWindow.Instance.WindowPanel, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //Panel.SetZIndex(MainContentWindow.Instance.WindowPanel, value.WindowState == WindowState.Normal ? 10 : 1);
            //Panel.SetZIndex(MainContentWindow.Instance.MaximizePanel, value.WindowState == WindowState.Normal ? 1 : 10);

            foreach (var items in _windowMapping)
            {
                if (items.Value == value) continue;

                if (items.Value.WindowState == WindowState.Normal &&
                    MainContentWindow.Instance.NormalPanel.Children.IndexOf(items.Value) != -1)
                {
                    MainContentWindow.Instance.NormalPanel.Children.Remove(items.Value);
                    MainContentWindow.Instance.WindowPanel.Children.Add(items.Value);
                }

                Panel.SetZIndex(items.Value, 1);
            }

            //ChangeListEvent.Invoke("");
        }

        public static void Maximize(MainContentWindow window, UserControl content)
        {
            if (!_windowMapping.TryGetValue(content, out var value))
                throw new InvalidOperationException("UI for this UserContext is not displayed");

            switch (value.WindowState)
            {
                case WindowState.Normal:
                    value.Width = Double.NaN;
                    value.Height = Double.NaN;
                    window.NormalPanel.Children.Remove(value);
                    window.WindowPanel.Children.Remove(value);
                    window.MaximizePanel.Children.Remove(value);
                    window.MaximizePanel.Children.Add(value);
                    Canvas.SetZIndex(window.MaximizePanel, 10);
                    Canvas.SetZIndex(window.NormalPanel, 1);
                    value.WindowState = WindowState.Maximized;
                    window.MaximizePanel.Visibility = window.MaximizePanel.Children.Count == 0
                        ? Visibility.Collapsed
                        : Visibility.Visible;
                    break;
                case WindowState.Minimized:
                    break;
                case WindowState.Maximized:
                    value.Width = value.BackupWidth;
                    value.Height = value.BackupHeight;
                    window.WindowPanel.Children.Remove(value);
                    window.MaximizePanel.Children.Remove(value);
                    window.NormalPanel.Children.Remove(value);
                    window.NormalPanel.Children.Add(value);
                    Canvas.SetZIndex(window.MaximizePanel, 1);
                    Canvas.SetZIndex(window.NormalPanel, 10);
                    value.WindowState = WindowState.Normal;
                    window.MaximizePanel.Visibility = window.MaximizePanel.Children.Count == 0
                        ? Visibility.Collapsed
                        : Visibility.Visible;
                    break;
            }

        }

        public static List<WindowSketch> GetListOfWindowSketches<T>() where T : UserControl
        {
            var result = (from items in _windowMapping
                          where items.Key.GetType() == typeof(T)
                          select new WindowSketch
                          {
                              Name = items.Value.Bar.Title,
                              Type = typeof(T),
                              Sketch = new VisualBrush(items.Value),
                              ContentSketch = new VisualBrush(items.Key)
                          }).ToList();

            return result;
        }

        public static List<WindowSketch> GetListOfWindowSketches(Type type)
        {
            var result = (from items in _windowMapping
                where items.Key.GetType() == type
                select new WindowSketch
                {
                    Name = items.Value.Bar.Title,
                    Type = type,
                    Sketch = new VisualBrush(items.Value),
                    ContentSketch = new VisualBrush(items.Key)
                }).ToList();

            return result;
        }

        public static void InfoToJson()
        {
            var list = _windowMapping.Select(items => new WindowInfo
            {
                ContentName = items.Key.GetType().FullName,
                Title = items.Value.Bar.Title,
                Width = items.Value.BackupWidth,
                Height = items.Value.BackupHeight,
                CanvasX = Canvas.GetLeft(items.Value),
                CanvasY = Canvas.GetTop(items.Value),
                CanvasZ = Canvas.GetZIndex(items.Value),
                Parent = (VisualTreeHelper.GetParent(items.Value) as Panel).Name,
                State = items.Value.WindowState
            })
                .ToList();

            var json = JsonConvert.SerializeObject(list);

            //var stream = File.Create(Directory.GetCurrentDirectory() + @"\windowsinfo.json");

            File.WriteAllText(Directory.GetCurrentDirectory() + @"\windowsinfo.json", json);
        }

        public static bool TryInitializeJson()
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + @"\windowsinfo.json";
                //var dinfo = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\windowsinfo.json");
                if (!File.Exists(path)) return false;
                var json = File.ReadAllText(path);
                var list = JsonConvert.DeserializeObject<List<WindowInfo>>(json);
                if (list.Count == 0) return false;
                foreach (var item in list)
                {
                    var wiw = new ContentWindow
                    {
                        ContentControl = { Content = CreateContentControl(item.ContentName) },
                        Height = Double.IsNaN(item.Height) || item.Parent == nameof(MainContentWindow.MaximizePanel) ? Double.NaN : item.Height,
                        Width = Double.IsNaN(item.Width) || item.Parent == nameof(MainContentWindow.MaximizePanel) ? Double.NaN : item.Width,
                        BackupHeight = item.Height,
                        BackupWidth = item.Width,
                        WindowState = item.State,
                        DataContext = new ContentWindowViewModel { Title = item.ContentName },
                        Bar = { Title = item.Title }

                    };

                    switch (item.Parent)
                    {
                        case nameof(MainContentWindow.NormalPanel):
                            MainContentWindow.Instance.NormalPanel.Children.Add(wiw);
                            SetPosition(wiw, item);
                            break;
                        case nameof(MainContentWindow.MaximizePanel):
                            MainContentWindow.Instance.MaximizePanel.Children.Add(wiw);
                            break;
                        case nameof(MainContentWindow.WindowPanel):
                            MainContentWindow.Instance.WindowPanel.Children.Add(wiw);
                            SetPosition(wiw, item);
                            break;
                    }

                    _windowMapping[(UserControl)wiw.ContentControl.Content] = wiw;

                    Canvas.SetZIndex(wiw, (int)item.CanvasZ);

                    if (item.CanvasZ > 1) ChangeTopMost((UserControl)wiw.ContentControl.Content);

                    //Canvas.SetZIndex(window.MaximizePanel, 1);

                    //Canvas.SetZIndex(window.WindowPanel, 1);
                }

                //ChangeListEvent.Invoke("");

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static UserControl CreateContentControl(string typeName)
        {
            var type = Type.GetType(typeName);
            return (UserControl)Activator.CreateInstance(type);
        }

        private static void SetPosition(UserControl wiw, WindowInfo source)
        {
            Canvas.SetLeft(wiw, source.CanvasX);
            Canvas.SetTop(wiw, source.CanvasY);
        }
    }
}