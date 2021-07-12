using System;
using Prism.Commands;
using Prism.Mvvm;
using Q.Services;
using Q.Views;

namespace Q.ViewModels
{
    public class KeyRegisterViewModel : BindableBase
    {
        private DelegateCommand _previousTabCommand;
        public DelegateCommand PreviousTabCommand => _previousTabCommand ??= new DelegateCommand(() =>
            { TabMappingService.ChangeTab<RegisterControl>(TabMappingService.GetVm<RegisterViewModel>(), PageMoveType.Previous); });

        private string _sixDigitCode;

        public string SixDigitCode
        {
            get => _sixDigitCode; 
            set => SetProperty(ref _sixDigitCode, value.ToUpper());
        }

        private DelegateCommand _clearTextCommand;
        public DelegateCommand ClearTextCommand => _clearTextCommand ??= new DelegateCommand(() => {SixDigitCode = string.Empty;});
    }
}