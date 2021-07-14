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
        private static readonly Dictionary<object, Button> IconsMapping = new Dictionary<object, Button>();

        public static void FastAddIcon<TUc, TVm>(string title,bool multipleWindows = true, string iconName = "") where TUc : UserControl where TVm : class, new()
        {
            ISketchIcon<TUc> icon = new SketchIcon<TUc> { Name = title };
            if(!TryAddIcon(icon, iconName == "" ? "Bug" : iconName, multipleWindows)) return;
            SetActionClick<TUc, TVm>(icon);
            AddBehavior((SketchIcon<TUc>)icon);
        }

        public static void FastAddIcon(string title, Type ucType, Type vmType , bool multipleWindows = true, string iconName = "")
        {
            var genericClass = typeof(SketchIcon<>);
            // MakeGenericType is badly named
            var constructedClass = genericClass.MakeGenericType(ucType);

            var created = Activator.CreateInstance(constructedClass);
            ((ISketchIcon<object>) created).Name = title;

            //var res = (SketchIcon<object>) created;
            //res.Name = title;

            if(!TryAddIcon(created, iconName == "" ? "Bug" : iconName, out var btn, vmType ,multipleWindows)) return;
            //SetActionClick(created);
            AddBehavior(created, btn);

            //ISketchIcon<TUc> icon = new SketchIcon<TUc> { Name = title };
            //if(!TryAddIcon(icon, iconName == "" ? "Bug" : iconName, multipleWindows)) return;
            //SetActionClick(created);
            //AddBehavior((SketchIcon<TUc>)icon);
        }

        public static bool TryAddIcon<TUc>(ISketchIcon<TUc> icon, string iconName, bool multipleWindows = true)
        {
            var name = icon.VmType.Name;
            if (IconsMapping.Count != 0 && IconsMapping.Select(item => ((ISketchIcon<object>)item.Key).ToType<TUc>()).Any(i => i?.VmType?.Name == name))
            {
               
                return false;
            }

            var btn = GetButton(iconName, icon, multipleWindows);

            if (btn == null)
            {
                
                return false;
            }

            MainContentWindow.Instance.IconsStackPanel.Children.Add(btn);
           
            IconsMapping[(ISketchIcon<object>)icon] = btn;

            
            return true;
        }

        public static bool TryAddIcon(object icon, string iconName, out Button button, Type vmType ,bool multipleWindows = true)
        {
            var si = (ISketchIcon<object>) icon;

            var intj = ((Type)si.GetType().GetInterfaces().First().GetProperty("VmType").GetValue(si)).Name;

            ////var name = icon.VmType.Name;
            if (IconsMapping.Count != 0 && IconsMapping.Select(item =>
                {
                    var (key, _) = item;
                    return ((key as ISketchIcon<object>)?.GetType()
                        .GetInterfaces().First().GetProperty("VmType")
                        ?.GetValue(key) as Type);
                })
                .Any(i => i.ToString() == intj))
            {
                button = null;
                return false;
            }

            var btn = GetButton(iconName, icon, multipleWindows, vmType);

            if (btn == null)
            {
                button = null;
                return false;
            }

            MainContentWindow.Instance.IconsStackPanel.Children.Add(btn);

            IconsMapping[(ISketchIcon<object>)icon] = btn;

            button = btn;
            //button = null;
            return true;
        }

        public static void AddBehavior<T>(SketchIcon<T> icon) where T : UserControl
        {
            if (!IconsMapping.TryGetValue(icon, out var btn))
                throw new InvalidOperationException("UI for this Sketch is not displayed!");

            var si = (ISketchIcon<object>) icon;

            var intj = si.GetType().GetInterfaces().First().GetProperty("VmType")?.GetValue(si) as Type;

            var behaviors = Interaction.GetBehaviors(btn);
            behaviors.Add(new SketchBehavior<T>(intj));
        }

        public static void AddBehavior(object icon, Button btn)
        {
            //if (!IconsMapping.TryGetValue((ISketchIcon<object>)icon, out var btn))
            //    throw new InvalidOperationException("UI for this Sketch is not displayed!");

            var behaviors = Interaction.GetBehaviors(btn);
            var si = (ISketchIcon<object>) icon;
            var intj = (Type)si.GetType().GetInterfaces().First().GetProperty("VmType").GetValue(si);
            behaviors.Add(new SketchBehavior(intj));
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

            try
            {
                var paths = PathsStorage.GetPath(iconName);
                if (paths.Count == 0) paths = PathsStorage.GetPath("Bug");

                var result = paths.First();

                if (result.Path != null)
                {
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

                    var path = paths.First().Path;

                    cvs2.Children.Add(path);

                    btn.Content = vb;
                }
                else
                {
                    var vb = result.ViewBox;
                    btn.Content = vb;
                }

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

        private static Button GetButton(string iconName, object icon, bool multipleWindows, Type vmType)
        {
            var btn = new Button
            {
                Background = new SolidColorBrush(Color.FromArgb(0,0,0,0)),
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
                        Stretch = Stretch.Uniform,
                        Width = 50,
                        Height = 50
                    };

                    var cvs1 = new Canvas {Height = 90, Width = 90};
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

                var si = (ISketchIcon<object>) icon;

                var action = (Action)(icon as ISketchIcon<object>).GetType().GetInterfaces().First().GetProperty("ClickAction").GetValue((ISketchIcon<object>) icon);

                //var action = (Action)icon.GetType().GetProperty("ClickAction").GetValue(icon);

                if (action == null)
                {
                    SetActionClick(icon, btn, vmType);
                    return btn;
                }

                btn.Click += (s, e) =>
                {
                    action();
                };

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

                WIW.ShowWindow( uc, 400, 600, icon.Name);

                MainContentWindowViewModel.Instance.Sketches = WIW.GetListOfWindowSketches<TUc>();
                MainContentWindowViewModel.Instance.SketchType = typeof(TUc).Name;
            };
        }

        public static void SetActionClick(object icon, Button btn, Type vmType)
        {
            //var si = icon as ISketchIcon<object>;

            //var action = si.GetType().GetInterfaces().First().GetProperty("ClickAction");

            var type = icon.GetType();
            var genType = type.GetGenericArguments().First();
            //var clickAction = icon.GetType().GetProperty(nameof(SketchIcon<object>.ClickAction));
            var name = icon.GetType().GetProperty(nameof(SketchIcon<object>.Name));
            ((ISketchIcon<object>)icon).ClickAction = () =>
            {
                //Проверка на возможность запуска нескольких окон
                if (!IconsMapping.TryGetValue(icon, out var btn))
                    throw new InvalidOperationException("UI for this SketchIcon is not displayed!");

                //Если такой возможности нет - не даем запустить больше одного окна
                if (!UIExtensions.GetMultipleWindows(btn))
                {
                    var list = WIW.GetListOfWindowSketches(type);

                    if(list.Count != 0) return;
                }

                //Создаем каркасный элемент и добавляем туда DataContext
                var uc = (UserControl)Activator.CreateInstance(genType);
                uc.DataContext = Activator.CreateInstance(vmType);

                WIW.ShowWindow( uc, 400, 600, name.GetValue(icon).ToString());

                MainContentWindowViewModel.Instance.Sketches = WIW.GetListOfWindowSketches(genType);
                MainContentWindowViewModel.Instance.SketchType = genType.Name;
            };

            btn.Click += (s,e) => ((ISketchIcon<object>) icon).ClickAction();

            //action.SetValue(si ,(System.Action)(() =>
            //{
            //    //Проверка на возможность запуска нескольких окон
            //    if (!IconsMapping.TryGetValue(icon, out var btn))
            //        throw new InvalidOperationException("UI for this SketchIcon is not displayed!");

            //    //Если такой возможности нет - не даем запустить больше одного окна
            //    if (!UIExtensions.GetMultipleWindows(btn))
            //    {
            //        var list = WIW.GetListOfWindowSketches(type);

            //        if(list.Count != 0) return;
            //    }

            //    //Создаем каркасный элемент и добавляем туда DataContext
            //    var uc = (UserControl)Activator.CreateInstance(type);
            //    uc.DataContext = Activator.CreateInstance(type);

            //    WIW.ShowWindow( uc, 400, 600, name.GetValue(icon).ToString());

            //    MainContentWindowViewModel.Instance.Sketches = WIW.GetListOfWindowSketches(type);
            //    MainContentWindowViewModel.Instance.SketchType = type.GetGenericArguments().First().Name;
            //}));
        }
    }
}