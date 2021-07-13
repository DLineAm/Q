using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Q.Services;

namespace Q.Views
{
    /// <summary>
    /// Логика взаимодействия для ContentWindow.xaml
    /// </summary>
    public partial class ContentWindow : UserControl, IView
    {
        public ContentWindow()
        {
            InitializeComponent();
        }

        public double BackupHeight;
        public double BackupWidth;

        public WindowState WindowState = WindowState.Normal;


        public void Close()
        {
            WIW.CloseWindow((UserControl)this.ContentControl.Content);
        }

        public void Minimize()
        {
            WIW.Minimize((UserControl)this.ContentControl.Content);
        }

        public void Maximize()
        {
            WIW.Maximize(MainContentWindow.Instance, (UserControl)this.ContentControl.Content);
        }

        private void ContentWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            WIW.ChangeTopMost((UserControl)this.ContentControl.Content);
        }
    }
}
