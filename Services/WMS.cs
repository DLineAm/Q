using System;
using System.Collections.Generic;
using System.Windows;

namespace Q.Services
{
    public class WMS
    {
        private static Dictionary<object, Window> _windowMapping = new Dictionary<object, Window>();

        public static void ShowWindow<TWin>(object vm) where TWin : Window
        {
            if(vm == null)
                throw new NullReferenceException("vm");
            if(_windowMapping.ContainsKey(vm))
                throw new InvalidOperationException("UI for this vm is already displayed");

            var window = (Window)Activator.CreateInstance(typeof(TWin));
            window.DataContext = vm;
            window.Show();
            _windowMapping[vm] = window;
        }

        public static void ShowModalWindow<TWin>(object vm) where TWin : Window
        {
            if(vm == null)
                throw new NullReferenceException("vm");
            if(_windowMapping.ContainsKey(vm))
                throw new InvalidOperationException("UI for this vm is already displayed");

            var window = (Window)Activator.CreateInstance(typeof(TWin));
            window.DataContext = vm;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Dispatcher.InvokeAsync(() => window.ShowDialog());
            _windowMapping[vm] = window;
        }

        public static void RegisterWindow(object vm, Window window)
        {
            if(_windowMapping.ContainsKey(vm))
                throw new InvalidOperationException("UI for this vm is already displayed");

            _windowMapping[vm] = window;
        }

        public static void CloseWindow(object vm)
        {
            if(!_windowMapping.ContainsKey(vm))
                throw new InvalidOperationException("UI for this vm is not displayed");

            Window window = null;
            _windowMapping.TryGetValue(vm, out window);

            window.Close();
            _windowMapping.Remove(vm);
        }
    }
}