using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Commands;
using Q.Services;

namespace Q.Views
{
    /// <summary>
    /// Логика взаимодействия для ContentWindowTitleBar.xaml
    /// </summary>
    public partial class ContentWindowTitleBar : UserControl
    {
        public ContentWindowTitleBar()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private DelegateCommand<IView> _closeCommand;
        public DelegateCommand<IView> CloseCommand => _closeCommand ??= new DelegateCommand<IView>(this.Close);

        private DelegateCommand<IView> _minimizeCommand;
        public DelegateCommand<IView> MinimizeCommand => _minimizeCommand ??= new DelegateCommand<IView>(this.Minimize);

        private DelegateCommand<IView> _maximizeCommand;
        public DelegateCommand<IView> MaximizeCommand => _maximizeCommand ??= new DelegateCommand<IView>(this.Maximize);

        public bool IsMaximize
        {
            get => (bool)GetValue(IsMaximizeProperty);
            set => SetValue(IsMaximizeProperty, value);
        }

        public static readonly DependencyProperty IsMaximizeProperty =
            DependencyProperty.Register("IsMaximizeEnabled", typeof(bool), typeof(ContentWindowTitleBar)
                , new PropertyMetadata(true));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ContentWindowTitleBar)
                , new PropertyMetadata(""));

        private void Maximize(IView view)
        {
            view.Maximize();
        }

        private void Close(IView view)
        {
            view.Close();
        }

        private void Minimize(IView view)
        {
            view.Minimize();
        }
    }
}
