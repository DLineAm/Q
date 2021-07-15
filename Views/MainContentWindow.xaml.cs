#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Q.Models;
using Q.Services;
using Q.ViewModels;
using QIPlugin;

namespace Q.Views
{
    /// <summary>
    /// Interaction logic for MainContentWindow.xaml
    /// </summary>
    public partial class MainContentWindow : Window, IView
    {
        //public static WIW WIW = new WIW();
        public MainContentWindow()
        {
            InitializeComponent();
            //WIW.Subscribe(this);
            Instance = this;

            try
            {
                //Assembly a = null;

                var a = Assembly.LoadFrom(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\" + "QTestUCPlugin\\bin\\Debug\\netcoreapp3.1\\QTestUCPlugin.dll")));

                var classType = a.GetType("QTestUCPlugin.Views.TestUC");

                var uc = (UserControl)Activator.CreateInstance(classType);

                var vmType = a.GetType("QTestUCPlugin.ViewModels.TestUCViewModel");
                var vm = Activator.CreateInstance(vmType);

                uc.DataContext = vm;

                IMS.FastAddIcon("TestUC", classType, vmType);

                //WIW.ShowWindow(uc, 400, 600, uc.GetType().Name);

                //IMS.FastAddIcon(uc.GetType().Name, uc, vm);

                //obj.DoWork();

                //var obj = Activator.CreateInstance(classType);
                //MethodInfo mi = classType.GetMethod("MyMethod");
                //mi.Invoke(obj, null);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        //public static void Subscribe()
        //{
        //    WIW.ChangeListEvent += m=>
        //    {
        //        ((MainContentWindowViewModel) Instance.DataContext).Sketches = WIW.GetListOfWindowSketches(m);
        //    };
        //}

        public static MainContentWindow Instance { get; private set; }

        public void Minimize()
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
                return;
            }

            this.WindowState = WindowState.Minimized;
        }

        public void Maximize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                //App.ChangeBorders(1);
                return;
            }

            this.WindowState = WindowState.Maximized;
            //App.ChangeBorders(7);
        }

        //private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var btn = (Button) sender;
        //    var uc = new RegisterControl(){DataContext = new RegisterViewModel()};
        //    WIW.ShowWindow(this, uc, 400, 600, UIExtensions.GetCustomTitle(btn));
        //}

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem {IsSelected: true} item)
            {
                var sketch = (WindowSketch)item.Content;
                var wiw = (ContentWindow) sketch.Sketch.Visual;
                var content =(UserControl) wiw.ContentControl.Content;
                if(wiw.WindowState == WindowState.Minimized) WIW.Minimize(content);
                WIW.ChangeTopMost(content);
                //SketchBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void MainContentWindow_OnClosing(object sender, CancelEventArgs e)
        {
            WIW.InfoToJson();
        }

        private void MainContentWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if(!WIW.TryInitializeJson()) return;
        }

        private void MainContentWindow_OnStateChanged(object? sender, EventArgs e)
        {
            MainBorder.CornerRadius = new CornerRadius(this.WindowState == WindowState.Maximized ? 0 : 6);
            MainBorder.BorderThickness = new Thickness(this.WindowState == WindowState.Maximized ? 7 : 1);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var context = btn?.DataContext as WindowSketch;
            var index = SketchProvider.GetIndexOfSketchList(context);
            if(index == -1) return;

            WIW.CloseWindow(index, context.Type);
        }
    }
}
