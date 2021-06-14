using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Q.Services;
using Q.Views;

namespace Q.ViewModels
{
    class RegisterViewModel : BindableBase
    {
        private DelegateCommand _signInCommand;
        public DelegateCommand SignInCommand => _signInCommand ??= new DelegateCommand(this.ChangePanel);

        private void ChangePanel()
        {
            TabMappingService.ChangeTab<LoginControl>(TabMappingService.GetVm<LoginViewModel>(), PageMoveType.Previous);
            //Cursors.IBeam
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _login;

        public string Login
        {
            get => _login; 
            set => SetProperty(ref _login, value);
        }

        private DelegateCommand<int?> _clearTextCommand;
        public DelegateCommand<int?> ClearTextCommand => _clearTextCommand ??= new DelegateCommand<int?>(this.ClearText);

        private void ClearText(int? textType) 
        {
            if (textType == null) return;
            switch (textType)
            {
                case 1:
                    Email = string.Empty;
                    break;
                case 0:
                    Login = string.Empty;
                    break;
            }
        }
    }
}
