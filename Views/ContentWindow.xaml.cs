using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
using Q.Annotations;
using Q.Services;
using static Q.App;

namespace Q.Views
{
    /// <summary>
    /// Логика взаимодействия для ContentWindow.xaml
    /// </summary>
    public partial class ContentWindow : UserControl, IView, INotifyPropertyChanged
    {
        public ContentWindow()
        {
            InitializeComponent();
            Contentt = this.ContentControl.Content;
        }

        private static object Contentt;

        public void SetContent(UserControl uc)
        {
            this.ContentControl.Content = uc;
            Contentt = this.ContentControl.Content;
        }

        public object GetContent() => Contentt;

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
            WIW.Maximize((UserControl)this.ContentControl.Content);
        }

        private void ContentWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            WIW.ChangeTopMost((UserControl)this.ContentControl.Content);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
