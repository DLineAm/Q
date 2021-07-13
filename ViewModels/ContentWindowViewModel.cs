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

        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
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