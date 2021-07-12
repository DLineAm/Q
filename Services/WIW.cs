using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Q.Models;
using Q.ViewModels;
using Q.Views;

namespace Q.Services
{
    public static class WIW
    {
        static WIW()
        {
            MainContentWindow.Subscribe();
        }

        private static readonly Dictionary<UserControl, ContentWindow> _windowMapping = new Dictionary<UserControl, ContentWindow>();

        public delegate void SketchHandler(string message);

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

        public static void ShowWindow(MainContentWindow window, UserControl content, double height, double width)
        {
            var wiw = new ContentWindow
            {
                ContentControl = { Content = content },
                Height = height,
                Width = width,
                BackupHeight = height,
                BackupWidth = width
            };

            window.NormalPanel.Children.Add(wiw);
            _windowMapping[content] = wiw;

            Canvas.SetZIndex(window.MaximizePanel, 1);

            Canvas.SetZIndex(window.WindowPanel, 1);

            Canvas.SetZIndex(window.NormalPanel, 10);

            ChangeTopMost(content);

            if (window.MaximizePanel.Children.Count == 0) window.MaximizePanel.Visibility = Visibility.Collapsed;

            ChangeListEvent.Invoke("");
        }

        public static void CloseWindow(MainContentWindow window, UserControl content)
        {
            if (!_windowMapping.TryGetValue(content, out var value))
                throw new InvalidOperationException("UI for this UserContext is not displayed");

            var parent = VisualTreeHelper.GetParent(value) as Panel;

            parent.Children.Remove(value);

            //window.WindowCanvas.Children.Remove(value);
            _windowMapping.Remove(content);
            ChangeListEvent.Invoke("");
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
                    window.MaximizePanel.Children.Add(value);
                    Canvas.SetZIndex(window.MaximizePanel, 10);
                    Canvas.SetZIndex(window.WindowPanel, 1);
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
                    window.NormalPanel.Children.Add(value);
                    Canvas.SetZIndex(window.MaximizePanel, 1);
                    Canvas.SetZIndex(window.WindowPanel, 10);
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
                          select new WindowSketch { Name = items.Key.GetType().Name, Type = typeof(T), Sketch = new VisualBrush(items.Value) }).ToList();

            return result;
        }
    }
}