using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Prism.Ioc;
using Q.Views;
using System.Windows;
using System.Windows.Controls;
using Q.Services;
using Q.ViewModels;
using QMappingServices;

namespace Q
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static string NameTitle = "Q | ";
        public static MainWindowViewModel Vm = new MainWindowViewModel();

        //public static IWIW WIW = GetWiw();

        //public static IIMS IMS = GetIms();


        protected override Window CreateShell()
        {
            var window = Container.Resolve<MainWindow>();
            TabMappingService.ChangeTab<LoginControl>(TabMappingService.GetVm<LoginViewModel>(), PageMoveType.None);
            window.DataContext = Vm;
            WMS.RegisterWindow(Vm, window);
            return window;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        private static IWIW GetWiw()
        {
            try
            {
                //Assembly a = null;

                var a = Assembly.LoadFrom(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\" + "QCore\\bin\\Debug\\netcoreapp3.1\\QCore.dll")));

                var classType = a.GetType("QCore.WIW");

                var wiw = (IWIW)Activator.CreateInstance(classType);

                return wiw;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        private static IIMS GetIms()
        {
            try
            {
                //Assembly a = null;

                var a = Assembly.LoadFrom(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\" + "QCore\\bin\\Debug\\netcoreapp3.1\\QCore.dll")));

                var classType = a.GetType("QCore.IMS");

                var ims = (IIMS)Activator.CreateInstance(classType);

                return ims;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
