using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Prism.Mvvm;
using Q.ViewModels;
using Q.Views;

namespace Q.Services
{
    public static class TabMappingService
    {
        private static Dictionary<Type, Type> _registeredTabs = new Dictionary<Type, Type>();

        private static LoginViewModel _loginViewModel;

        private static RegisterViewModel _registerViewModel;

        public static void RegisterTab<TWin, TVm>() where TWin : UserControl where  TVm : BindableBase
        {
            _registeredTabs[typeof(TWin)] = typeof(TVm);
        }

        public static void ChangeTab<TWin>(BindableBase vm) where TWin : UserControl
        {
            var tab = (UserControl)Activator.CreateInstance(typeof(TWin));
            App.Vm.CurrentView = tab;
            ((UserControl)App.Vm.CurrentView).DataContext = vm;
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