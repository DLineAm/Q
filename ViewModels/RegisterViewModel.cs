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
            TabMappingService.ChangeTab<LoginControl>(TabMappingService.GetVm<LoginViewModel>());
            //Cursors.IBeam
        }
    }
}
