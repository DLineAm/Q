using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Xaml.Behaviors;
using Q.Models;
using Q.ViewModels;
using Q.Views;

namespace Q.Services
{
    public static class IMS
    {
        private static readonly List<ISketchIcon<object>> IconsMapping = new List<ISketchIcon<object>>();

        public static bool TryAddIcon<T>(ISketchIcon<T> icon, string iconName)
        {
            //Console.WriteLine(icon.VmType.Name);
            //MessageBox.Show(icon.VmType.Name);
            var name = icon.VmType.Name;
            if (IconsMapping.Count != 0 && IconsMapping.Select(item => item.ToType<T>()).Any(i => i?.VmType?.Name == name))
            {
                return false;
            }
            //_iconsMapping.ForEach(p => MessageBox.Show(p.ToType<T>()?.Name));
            //if (_iconsMapping.Any(p => p.ToType<T>()?.Name == name))
            //    return false;

            var btn = GetButton(iconName, icon);

            if (btn == null) return false;

            MainContentWindow.Instance.IconsStackPanel.Children.Add(btn);
            var behaviors = Interaction.GetBehaviors(btn);
            behaviors.Add(new SketchBehavior());
            IconsMapping.Add((ISketchIcon<object>)icon);

            return true;
        }

        private static Button GetButton<T>(string iconName, ISketchIcon<T> icon)
        {
            var btn = new Button
            {
                Background = new SolidColorBrush(Color.FromArgb(0,0,0,0)),
                BorderThickness = new Thickness(0),
                Margin = new Thickness(0)
            };

            var vb = new Viewbox
            {
                Stretch = Stretch.Uniform,
                Width = 50,
                Height = 50
            };

            var cvs1 = new Canvas {Height = 90, Width = 90};
            var cvs2 = new Canvas();

            vb.Child = cvs1;
            cvs1.Children.Add(cvs2);

            try
            {
                var paths = PathsStorage.GetPath(iconName);
                if (paths.Count == 0) paths = PathsStorage.GetPath("Bug");
                var path = paths.First().Path;

                cvs2.Children.Add(path);

                btn.Content = vb;
                btn.Click += (s,e) =>
                    icon.ClickAction();

                UIExtensions.SetCustomTitle(btn, icon.Name);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
            

            return btn;

            //    <Viewbox Stretch="Uniform"
            //Width="50"
            //Height="50">
            //    <Canvas Width="90"
            //Height="90">

            //    <Canvas>
        }

        public static void SetActionClick<T>(ISketchIcon<T> icon) where T : UserControl
        {
            icon.ClickAction = () =>
            {
                //var btn = (Button) sender;
                var uc = (UserControl)Activator.CreateInstance<T>();
                uc.DataContext = new RegisterViewModel();
                WIW.ShowWindow(MainContentWindow.Instance, uc, 400, 600, icon.Name);
            };
        }
    }

    public struct SketchIcon<TC> : ISketchIcon<TC>
    {
        public string Name { get; set; }
        public Action ClickAction { get; set; }
    }

    public interface ISketchIcon<out TC>
    {
        public string Name { get; set; }

        public Type VmType => typeof(TC);

        public Action ClickAction { get; set; }

        public ISketchIcon<T> ToType<T>()
        {
            try
            {
                var result = (ISketchIcon<T>)this;

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}