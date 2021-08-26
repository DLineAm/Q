using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
using QCore;

namespace Q.Views
{
    /// <summary>
    /// Логика взаимодействия для ContentWindow.xaml
    /// </summary>
    public partial class ContentWindow : UserControl, IView, INotifyPropertyChanged, IServiceBaseContainer
    {
        //public bool WindowIsFocused = false;

        public ContentWindow()
        {
            InitializeComponent();
            Contentt = this.ContentControl.Content;
            Test = true;
        }

        private bool _isDragging = false;
        private bool _isStretching = false;
        private bool _stretchLeft = false;
        private bool _stretchRight = false;

        private IInputElement _inputElement;

        public IInputElement InputElement
        {
            get => _inputElement;
            set
            {
                this._inputElement = value;
                this._isDragging = false;
                this._isStretching = false;
            }
        }

        private static object Contentt;

        public void SetContent(UserControl uc)
        {
            this.ContentControl.Content = uc;
            Contentt = this.ContentControl.Content;
        }

        public object GetContent() => Contentt;

        public double BackupHeight { get; set; }
        public double BackupWidth { get; set; }

        public double BackupX { get; set; }
        public double BackupY { get; set; }

        public bool IsDragging
        {
            get =>_isDragging;
            set
            {
                this._isDragging = value;
                this._isStretching = !this._isDragging;
            }
        }

        public bool IsStretching
        {
            get => _isStretching;
            set
            {
                this._isStretching = value;
                this.IsDragging = !this._isStretching;
            }
        }

        public bool StretchLeft
        {
            get => _stretchLeft; 
            set { this._stretchLeft = value; this._stretchRight = !this._stretchLeft; }
        }
        public bool StretchRight
        {
            get => _stretchRight;
            set { this._stretchRight = value; this._stretchLeft = !this._stretchRight; }
        }

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

        public bool? IsVisibleWindow { get; set; } = null;
        public bool? IsNormalState { get; set; } = true;

        public bool WindowIsFocused { get; set; }
        public string Title { get => Bar.Title; set => Bar.Title = value; }

        public void SetIsVisible(bool? value)
        {
            IsVisibleWindow = value;
            this.Visibility = BoolToVisibility(value);
        }

        public void SetState(bool? isNormal)
        {
            IsNormalState = isNormal;
            this.WindowState = BoolToState(isNormal);
        }

        private static Visibility BoolToVisibility(bool? value) => value switch
        {
            true => Visibility.Visible,
            false => Visibility.Hidden,
            _ => Visibility.Collapsed
        };

        private static WindowState BoolToState(bool? value) => value switch
        {
            true => WindowState.Normal,
            false => WindowState.Maximized,
            _ => WindowState.Minimized
        };


        public bool Test { get; set; }
    }
}
