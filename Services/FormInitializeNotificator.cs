using System;
using Q.Views;

namespace Q.Services
{
    public static class FormInitializeNotificator
    {
        public static void Subscribe(Action action)
        {
            MainContentWindow.Instance.InitializeEvent += () => action();
        }

        public static void Invoke()
        {
            MainContentWindow.Instance.InvokeInitializeEvent();
        }
    }
}