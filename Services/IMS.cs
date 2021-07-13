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
        private static readonly Dictionary<ISketchIcon<object>, Button> IconsMapping = new Dictionary<ISketchIcon<object>, Button>();

        public static void FastAddIcon<TUc, TVm>(string title, bool multipleWindows = true, string iconName = "") where TUc : UserControl where TVm : class, new()
        {
            ISketchIcon<TUc> icon = new SketchIcon<TUc> { Name = title };
            if(!TryAddIcon(icon, iconName == "" ? "Bug" : iconName, multipleWindows)) return;
            SetActionClick<TUc, TVm>(icon);
            AddBehavior(new SketchBehavior<TUc>(), (SketchIcon<TUc>)icon);
        }

        public static bool TryAddIcon<TUc>(ISketchIcon<TUc> icon, string iconName, bool multipleWindows = true)
        {
            var name = icon.VmType.Name;
            if (IconsMapping.Count != 0 && IconsMapping.Select(item => item.Key.ToType<TUc>()).Any(i => i?.VmType?.Name == name))
            {
                return false;
            }

            var btn = GetButton(iconName, icon, multipleWindows);

            if (btn == null) return false;

            MainContentWindow.Instance.IconsStackPanel.Children.Add(btn);
           
            IconsMapping[(ISketchIcon<object>)icon] = btn;

            return true;
        }

        public static void AddBehavior<T>(SketchBehavior<T> behavior, SketchIcon<T> icon) where T : UserControl
        {
            if (!IconsMapping.TryGetValue(icon, out var btn))
                throw new InvalidOperationException("UI for this Sketch is not displayed!");

            var behaviors = Interaction.GetBehaviors(btn);
            behaviors.Add(new SketchBehavior<T>());
        }

        private static Button GetButton<T>(string iconName, ISketchIcon<T> icon, bool multipleWindows)
        {
            var btn = new Button
            {
                Background = new SolidColorBrush(Color.FromArgb(0,0,0,0)),
                BorderThickness = new Thickness(0),
                Margin = new Thickness(0)
            };

            UIExtensions.SetMultipleWindows(btn, multipleWindows);

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
                btn.Click += (s, e) =>
                {
                    icon.ClickAction();
                };

                UIExtensions.SetCustomTitle(btn, icon.Name);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }

            return btn;
        }

        public static void SetActionClick<TUc, TVm>(ISketchIcon<TUc> icon) where TUc : UserControl where TVm : class, new()
        {
            icon.ClickAction = () =>
            {
                //Проверка на возможность запуска нескольких окон
                if (!IconsMapping.TryGetValue(icon, out var btn))
                    throw new InvalidOperationException("UI for this SketchIcon is not displayed!");

                //Если такой возможности нет - не даем запустить больше одного окна
                if (!UIExtensions.GetMultipleWindows(btn))
                {
                    var list = WIW.GetListOfWindowSketches<TUc>();

                    if(list.Count != 0) return;
                }

                //Создаем каркасный элемент и добавляем туда DataContext
                var uc = (UserControl)Activator.CreateInstance<TUc>();
                uc.DataContext = Activator.CreateInstance<TVm>();

                WIW.ShowWindow(MainContentWindow.Instance, uc, 400, 600, icon.Name);

                MainContentWindowViewModel.Instance.Sketches = WIW.GetListOfWindowSketches<TUc>();
                MainContentWindowViewModel.Instance.SketchType = typeof(TUc).Name;
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