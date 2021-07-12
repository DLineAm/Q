using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Q.Models;
using Q.Services;
using Q.ViewModels;

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
        }

        public static void Subscribe()
        {
            WIW.ChangeListEvent += m =>
            {
                ((MainContentWindowViewModel) Instance.DataContext).Sketches = WIW.GetListOfWindowSketches<LoginControl>();
            };
        }

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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var uc = new LoginControl{DataContext = new LoginViewModel()};
            WIW.ShowWindow(this, uc, 400, 600);
        }

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
    }
}
