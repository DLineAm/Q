using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Mvvm;
using Q.Services;
using Q.Views;

namespace Q.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        

        private string _title = "Окно авторизации";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MainWindowViewModel()
        {
            
        }

        private bool _isEnabled = true;
        public bool IsEnabled { get => _isEnabled; set => SetProperty(ref _isEnabled, value); }

        private PageMoveType _pageMoveType = PageMoveType.None;
        public PageMoveType PageMoveType { get => _pageMoveType; set => SetProperty(ref _pageMoveType, value); }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        private object _currentView;
        public object CurrentView { get => _currentView; set => SetProperty(ref _currentView, value); }

        private object _nextView;
        public object NextView { get => _nextView; set => SetProperty(ref _nextView, value); }

        private object _previousView;
        public object PreviousView { get => _previousView; set => SetProperty(ref _previousView, value); }

    }
}
