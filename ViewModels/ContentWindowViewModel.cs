using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Q.Annotations;

namespace Q.ViewModels
{
    public class ContentWindowViewModel : INotifyPropertyChanged
    {
        
        //private Visibility _windowVisibility = Visibility.Visible;
        //public Visibility WindowVisibility { get => _windowVisibility; set => SetProperty(ref _windowVisibility, value); }

        private int _canvasX;

        public int CanvasX
        {
            get => _canvasX;
            set => SetProperty(ref _canvasX, value);
        }

        private int _canvasY;

        public int CanvasY
        {
            get => _canvasY;
            set => SetProperty(ref _canvasY, value);
        }

        private int _canvasZ;

        public int CanvasZ
        {
            get => _canvasZ;
            set => SetProperty(ref _canvasZ, value);
        }

        private void SetProperty<T>(ref T source, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(source, value))
                return;
            source = value;
            this.OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}