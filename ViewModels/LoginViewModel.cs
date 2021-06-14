using System;
using System.Windows;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Mvvm;
using Q.Services;
using Q.Views;

namespace Q.ViewModels
{
    class LoginViewModel : BindableBase
    {
        private readonly DispatcherTimer _loadingTimer = new DispatcherTimer{Interval = TimeSpan.FromSeconds(3)};

        public LoginViewModel()
        {
            _loadingTimer.Tick += (s, e) => App.Vm.IsLoading = false;
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _login;

        public string Login
        {
            get => _login; 
            set => SetProperty(ref _login, value);
        }

        private string _errorText;

        public string ErrorText
        {
            get => _errorText; 
            set => SetProperty(ref _errorText, value);
        }

        private Visibility _errorVisibility = Visibility.Collapsed;

        public Visibility ErrorVisibility { get => _errorVisibility; set => SetProperty(ref _errorVisibility, value); }

         private DelegateCommand<int?> _clearTextCommand;
        public DelegateCommand<int?> ClearTextCommand => _clearTextCommand ??= new DelegateCommand<int?>(this.ClearText);

        private void ClearText(int? textType) 
        {
            if (textType == null) return;
            switch (textType)
            {
                case 1:
                    Password = string.Empty;
                    break;
                case 0:
                    Login = string.Empty;
                    break;
            }
        }

        private DelegateCommand _signinCommand;
        public DelegateCommand SigninCommand => _signinCommand ??= new DelegateCommand(this.Signin);

        private DelegateCommand _registerCommand;
        public DelegateCommand RegisterCommand => _registerCommand ??= new DelegateCommand(this.ChangePanel);

        private void ChangePanel()
        {
            TabMappingService.ChangeTab<RegisterControl>(TabMappingService.GetVm<RegisterViewModel>());
        }

        private void Signin(/*object param*/)
        {
            //if (!(param is PasswordBox passwordBox))
            //    return;
            //Password = passwordBox.Password;
            if (string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Login))
            {
                SetError("Оба поля должны быть заполнены!");
                return;
            }

            if (Password.Length < 6 || Password.Length > 16 ||
                Login.Length < 6 || Login.Length > 16)
            {
                SetError("Логин и пароль должны иметь длинну более 6 и менее 16 символов!");
                return;
            }
            ErrorVisibility = Visibility.Collapsed;
            App.Vm.IsLoading = true;
            _loadingTimer.Start();
        }

        private void SetError(string text)
        {
            ErrorVisibility = Visibility.Visible;
            ErrorText = text;
        }
    }
}
