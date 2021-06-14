using Prism.Ioc;
using Q.Views;
using System.Windows;
using Q.Services;
using Q.ViewModels;

namespace Q
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static MainWindowViewModel Vm = new MainWindowViewModel();
        protected override Window CreateShell()
        {
            var window = Container.Resolve<MainWindow>();
            TabMappingService.ChangeTab<LoginControl>(TabMappingService.GetVm<LoginViewModel>(), PageMoveType.None);
            window.DataContext = Vm;
            return window;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
