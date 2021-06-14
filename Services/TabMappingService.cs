using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Prism.Mvvm;
using Q.ViewModels;
using Q.Views;

namespace Q.Services
{
    public static class TabMappingService
    {
        private static DispatcherTimer _changeTabTimer = new DispatcherTimer(DispatcherPriority.Render) {Interval = TimeSpan.FromSeconds(0.6)};
        private static readonly Dictionary<Type, Type> _registeredTabs = new Dictionary<Type, Type>();

        private static UserControl queueControl;

        private static LoginViewModel _loginViewModel;

        private static RegisterViewModel _registerViewModel;

        static TabMappingService()
        {
            _changeTabTimer.Tick += (s, e) =>
            {
                var a = queueControl;
                App.Vm.CurrentView = a;
                App.Vm.PageMoveType = PageMoveType.None;
                App.Vm.IsEnabled = true;
                _changeTabTimer.Stop();
                //_changeTabTimer = new DispatcherTimer{Interval = TimeSpan.FromSeconds(1)};
                //_changeTabTimer
            };
        }

        public static void RegisterTab<TWin, TVm>() where TWin : UserControl where  TVm : BindableBase
        {
            _registeredTabs[typeof(TWin)] = typeof(TVm);
        }

        public static void ChangeTab<TWin>(BindableBase vm, PageMoveType pmt) where TWin : UserControl
        {
            var tab = (UserControl)Activator.CreateInstance(typeof(TWin));
            tab.DataContext = vm;
            queueControl = tab;
            switch (pmt)
            {
                case PageMoveType.Previous:
                    App.Vm.PreviousView = tab;
                    break;
                case PageMoveType.None:
                    App.Vm.CurrentView = tab;
                    break;
                case PageMoveType.Next:
                    App.Vm.NextView = tab;
                    break;
            }

            App.Vm.IsEnabled = false;
            //App.Vm.CurrentView = tab;
            App.Vm.PageMoveType = pmt;
            if (pmt != PageMoveType.None)
            {
                _changeTabTimer.Start();
                return;
            }

            App.Vm.IsEnabled = true;
        }

        public static BindableBase GetVm<T>( ) where T : BindableBase
        {
            return typeof(T) switch
            {
                { } loginType when loginType == typeof(LoginViewModel) => _loginViewModel ??= new LoginViewModel(),
                { } registerType when registerType == typeof(RegisterViewModel) => _registerViewModel ??= new RegisterViewModel(),
                _ => null
            };
        } 
    }
}