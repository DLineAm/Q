#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using Q.Models;
using Q.Services;
using Q.ViewModels;
using QCore;
using QIPlugin;
using Point = System.Drawing.Point;

namespace Q.Views
{
    /// <summary>
    /// Interaction logic for MainContentWindow.xaml
    /// </summary>
    public partial class MainContentWindow : Window, IView, IServiceWindow
    {
        public static IContainer<MainContentWindow> Container { get; set; } = new MainContentWindowContainer();
        //public static WIW WIW = new WIW();
        public MainContentWindow()
        {
            InitializeComponent();
            //WIW.Subscribe(this);
            Instance = this;

            ((MainContentWindowContainer) Container!)!.Test = (int) this.Width;

            Loaded += OnLoaded;

            try
            {
                //Assembly a = null;

                //var a = Assembly.LoadFrom(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\" + "QTestUCPlugin\\bin\\Debug\\netcoreapp3.1\\QTestUCPlugin.dll")));

                //var classType = a.GetType("QTestUCPlugin.Views.TestUC");

                //var uc = (UserControl)Activator.CreateInstance(classType);

                //var vmType = a.GetType("QTestUCPlugin.ViewModels.TestUCViewModel");
                //var vm = Activator.CreateInstance(vmType);

                //uc.DataContext = vm;

                //IMS.FastAddIcon("TestUC", classType, vmType);

                //WIW.ShowWindow(uc, 400, 600, uc.GetType().Name);

                //IMS.FastAddIcon(uc.GetType().Name, uc, vm);

                //obj.DoWork();

                //var obj = Activator.CreateInstance(classType);
                //MethodInfo mi = classType.GetMethod("MyMethod");
                //mi.Invoke(obj, null);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //var a = Assembly.LoadFrom(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\" + "QTestUCPlugin\\bin\\Debug\\netcoreapp3.1\\QTestUCPlugin.dll")));

            //var classType = a.GetType("QTestUCPlugin.Views.TestUC");

            //var uc = (UserControl)Activator.CreateInstance(classType);

            //var vmType = a.GetType("QTestUCPlugin.ViewModels.TestUCViewModel");
            //var vm = Activator.CreateInstance(vmType);

            //uc.DataContext = vm;

            //IMS.FastAddIcon("TestUC", classType, vmType);

            //WIW.ShowWindow(uc, 400, 600, uc.GetType(), uc.GetType().Name);

            //IMS.FastAddIcon(uc.GetType().Name, uc.GetType(), vm.GetType());
        }

        //public static void Subscribe()
        //{
        //    WIW.ChangeListEvent += m=>
        //    {
        //        ((MainContentWindowViewModel) Instance.DataContext).Sketches = WIW.GetListOfWindowSketches(m);
        //    };
        //}

        public void SetLeft(string panelName, int count, UserControl uc)
        {
            switch (panelName)
            {
                case nameof(NormalPanel):
                    Canvas.SetLeft(uc, count);
                    //NormalPanel.Children.Add()
                    break;
                case nameof(MaximizePanel):
                    Canvas.SetZIndex(NormalPanel, count);
                    break;
                case nameof(WindowPanel):
                    Canvas.SetZIndex(NormalPanel, count);
                    break;
            }
        }

        public static MainContentWindow Instance { get; private set; }

        public void Minimize()
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
                return;
            }

            this.WindowState = WindowState.Minimized;
        }

        public void Maximize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                //App.ChangeBorders(1);
                return;
            }

            this.WindowState = WindowState.Maximized;
            //App.ChangeBorders(7);
        }

        //private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var btn = (Button) sender;
        //    var uc = new RegisterControl(){DataContext = new RegisterViewModel()};
        //    WIW.ShowWindow(this, uc, 400, 600, UIExtensions.GetCustomTitle(btn));
        //}

        public List<IServiceBaseContainer> GetElementCollection(string panelName)
        {
            switch (panelName)
            {
                case nameof(NormalPanel):
                    return this.NormalPanel.Children.Cast<UIElement>().ToList().Cast<IServiceBaseContainer>().ToList();
                case nameof(MaximizePanel):
                    return this.MaximizePanel.Children.Cast<UIElement>().ToList().Cast<IServiceBaseContainer>().ToList();
                case nameof(WindowPanel):
                    return this.WindowPanel.Children.Cast<UIElement>().ToList().Cast<IServiceBaseContainer>().ToList();
                default: return null!;
            }
        }

        public void SetZIndex(string panelName, int count)
        {
            switch (panelName)
            {
                case nameof(NormalPanel):
                    Panel.SetZIndex(NormalPanel, count);
                    break;
                case nameof(MaximizePanel):
                    Panel.SetZIndex(NormalPanel, count);
                    break;
                case nameof(WindowPanel):
                    Panel.SetZIndex(NormalPanel, count);
                    break;
            }
        }

        public int GetZIndexChildren(object children)
        {
            if (children is UIElement element)
            {
                return Canvas.GetZIndex(element);
            }
            throw new InvalidOperationException("children must be a UIElement type");
        }

        public void SetZIndexChildren(object children, int count)
        {
            if (children is UIElement element)
            {
                Panel.SetZIndex(element, 1);
                return;
            }

            throw new InvalidOperationException("children must be a UIElement type");
        }

        public void AddChildrenToPanel(string panelName, object element)
        {
            if(element is UIElement elem)
            {
                switch (panelName)
                {
                    case nameof(NormalPanel):
                        NormalPanel.Children.Add(elem);
                        break;
                    case nameof(MaximizePanel):
                        MaximizePanel.Children.Add(elem);
                        break;
                    case nameof(WindowPanel):
                        WindowPanel.Children.Add(elem);
                        break;
                }
                return;
            }
            throw new InvalidOperationException("element must be a UIElement type");
        }

        public void ReplaceElement(string newPanelName, object element)
        {
            if (element is UIElement elem)
            {
                WindowPanel.Children.Remove(elem);
                MaximizePanel.Children.Remove(elem);
                NormalPanel.Children.Remove(elem);
                AddChildrenToPanel(newPanelName, elem);
                return;
            }
            throw new InvalidOperationException("element must be a UIElement type");
            
        }

        public object GetButton(string iconName, object icon, bool multipleWindows, Type vmType)
        {
            var btn = new Button
            {
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                BorderThickness = new Thickness(0),
                Margin = new Thickness(0)
            };
            UIExtensions.SetMultipleWindows(btn, multipleWindows);

            try
            {
                var paths = PathsStorage.GetPath(iconName);
                if (paths.Count == 0) paths = PathsStorage.GetPath("Bug");

                var result = paths.First();

                if (result.Path != null)
                {
                    var vb = new Viewbox
                    {
                        Stretch = Stretch.Fill,
                        Width = 90,
                        Height = 90
                    };

                    var cvs1 = new Canvas { Height = 90, Width = 90 };
                    var cvs2 = new Canvas();

                    vb.Child = cvs1;
                    cvs1.Children.Add(cvs2);

                    var path = paths.First().Path;

                    cvs2.Children.Add(path);

                    btn.Content = vb;
                }
                else
                {
                    var vb = result.ViewBox;
                    btn.Content = vb;
                }

                var name = icon.GetType().GetProperty(nameof(SketchIcon<object>.Name)).GetValue(icon);

                UIExtensions.SetCustomTitle(btn, name);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }

            return btn;
        }

        public void SetButtonClick(object btn, Action action)
        {
            if (btn is Button button)
            {
                button.Click += (s, e) => { action(); };
                return;
            }
            

            throw new InvalidOperationException("button must be a Button type");
        }

        public void SetButtonClickAction(Action action)
        {
            
        }

        public bool GetMultipleWindowsProperty(object button)
        {
            if (button is DependencyObject obj)
            {
                return UIExtensions.GetMultipleWindows(obj);
            }

            throw new InvalidOperationException("button must be a DependencyObject type");
        }

        public object GetElementContent(object element)
        {
            if (element is Button btn)
                return btn.Content;

            if (element is UserControl uc)
                return uc.Content;

            throw new InvalidOperationException("button must be a Button type");
        }

        public object GetElementDataContext(object element)
        {
            if (element is UserControl uc) return uc.DataContext;
            throw new InvalidOperationException("element must be a UserControl type");
        }

        public Point GetCanvasPosition(IServiceBaseContainer container)
        {
            try
            {
                var result = new Point();
                result.X = Convert.ToInt32(Canvas.GetLeft((UIElement)container));
                result.Y = Convert.ToInt32(Canvas.GetTop((UIElement)container));
                return result;
            }
            catch
            {
                return new Point(0, 0);
            }
        }

        public void SetCanvasPosition(IServiceBaseContainer container, double x, double y)
        {
            if (container is UIElement element)
            {
                Canvas.SetLeft(element, x);
                Canvas.SetTop(element, y);
                return;
            }
            throw new InvalidOperationException("container must be a UIElement type");
        }

        public Type GetTypeOfDataContext(object userControl)
        {
            if (userControl is UserControl control)
            {
                return control.DataContext.GetType();
            }
            throw new InvalidOperationException("userControl must be a UserControl type");
        }

        public void ChangePanelVisibility(string panelName, bool? visibility)
        {
            var result = BoolToVisibility(visibility);
            switch (panelName)
            {
                case nameof(NormalPanel):
                    NormalPanel.Visibility = result;
                    break;
                case nameof(MaximizePanel):
                    MaximizePanel.Visibility = result;
                    break;
                case nameof(WindowPanel):
                    WindowPanel.Visibility = result;
                    break;
            }
        }

        public void AddBehavior<T>(object button, Type type)
        {
            if (button is Button elem)
            {
                var behaviors = Interaction.GetBehaviors(elem);
                behaviors.Add(new SketchBehavior<T>(type));
            }
        }

        public void AddBehavior(object icon, object button)
        {
            if (button is Button elem)
            {
                var si = (ISketchIcon<object>)icon;
                var intj = (Type)si.GetType().GetInterfaces().First().GetProperty("VmType").GetValue(si);
                var behaviors = Interaction.GetBehaviors(elem);
                behaviors.Add(new SketchBehavior(intj));
                return;
            }
            throw new InvalidOperationException("button must be a Button type");
        }

        public string GetIconName(object viewBox)
        {
            if (viewBox is Viewbox vb)
            {
                return PathsStorage.GetIconName(vb);
            }
            throw new InvalidOperationException("viewBox must be a ViewBox type");
        }

        public string GetParentName(object parent)
        {
            if (parent is Panel panel)
                return panel.Name;
            if (parent is ContentWindow window)
            {
                var visualTreeParent = VisualTreeHelper.GetParent(window) as Panel;
                if (visualTreeParent?.Name != null) return visualTreeParent.Name;
            }
            return "";
        }

        public void SetFocusedElement(object parent, object child)
        {
            if (parent is DependencyObject dependencyObject && child is IInputElement element)
            {
                FocusManager.SetFocusedElement(dependencyObject, element);
            }
            else
            {
                throw new InvalidOperationException("parent must be DependencyObject type, child must be IInputElement type");
            }
        }

        public bool RemoveFromPanel(object content, out object parent)
        {
            try
            {
                var panel = VisualTreeHelper.GetParent((DependencyObject)content) as Panel;
                panel.Children.Remove((UIElement)content);
                parent = panel;
                return true;
            }
            catch
            {
                parent = null!;
                return false;
            }
        }

        private static Visibility BoolToVisibility(bool? value) => value switch
        {
            true => Visibility.Visible,
            false => Visibility.Hidden,
            _ => Visibility.Collapsed
        };

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem {IsSelected: true} item)
            {
                var sketch = (WindowSketch)item.Content;
                var wiw = (ContentWindow) sketch.Sketch.Visual;
                var content =(UserControl) wiw.ContentControl.Content;
                if(wiw.WindowState == WindowState.Minimized) WIW.Minimize(content);
                WIW.ChangeTopMost(content);
                //SketchBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void MainContentWindow_OnClosing(object sender, CancelEventArgs e)
        {
            WIW.InfoToJson();
        }

        private void MainContentWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if(!WIW.TryInitializeJson()) return;
        }

        private void MainContentWindow_OnStateChanged(object? sender, EventArgs e)
        {
            MainBorder.CornerRadius = new CornerRadius(this.WindowState == WindowState.Maximized ? 0 : 6);
            MainBorder.BorderThickness = new Thickness(this.WindowState == WindowState.Maximized ? 7 : 1);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var context = btn?.DataContext as WindowSketch;
            var index = SketchProvider.GetIndexOfSketchList(context);
            if(index == -1) return;

            WIW.CloseWindow(index, context.Type);
        }

        //public IServiceBaseContainer GetBaseContainer(double height, double width, object dataContext, object content, string title)
        //{
        //    var result = new ContentWindow
        //    {
        //        Height = height,
        //        Width = width,
        //        BackupHeight = height,
        //        BackupWidth = width,
        //        DataContext = dataContext,
        //        ContentControl = {Content = content},
        //        Bar = {Title = title}
        //    };
        //    //result.DataContext = new ContentWindowViewModel{Title = title};
        //    return result;
        //}
        public IServiceBaseContainer GetBaseContainer(double height, double width, double backupHeight, double backupWidth,
            bool? isNormalState, object dataContext, object content, string title)
        {
            var result = new ContentWindow
            {
                Height = height,
                Width = width,
                BackupHeight = backupHeight,
                BackupWidth = backupWidth,
                //DataContext = dataContext,
                IsNormalState = isNormalState,
                Bar = {Title = title},
                DataContext = new ContentWindowViewModel {Title = title}
            };
            result.SetContent((UserControl)content);
            return result;
        }
    }
}
