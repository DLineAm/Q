using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.Xaml.Behaviors;

using Q.ViewModels;
using Q.Views;

namespace Q.Services
{
    public class SketchBehavior<T> : SketchBehavior where T : UserControl
    {
        private Canvas canvas;

        private Panel panel;

        public SketchBehavior(Type type) : base(type)
        {

        }

        protected override void OnAttached()
        {
            base.OnAttached();

            // Присоединение обработчиков событий            
            //this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;

            panel ??= VisualTreeHelper.GetParent(this.AssociatedObject) as Panel;
            //MessageBox.Show(VisualTreeHelper.GetParent(this.AssociatedObject).GetType().Name);
            //this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            // Удаление обработчиков событий
            //this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            //this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
        }

        // Отслеживание перетаскивания элемента
        private bool isDragging = false;

        // Запись точной позиции, в которой нажата кнопка
        private Point mouseOffset = new Point(85, 0);

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            // Поиск canvas
            canvas ??= VisualTreeHelper.GetParent(this.AssociatedObject) as Canvas;

            // Режим перетаскивания
            isDragging = true;

            // Получение позиции нажатия относительно элемента
            mouseOffset = e.GetPosition(AssociatedObject);

            // Захват мыши
            AssociatedObject.CaptureMouse();
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (panel == null) return;
            // Получение позиции элемента относительно Canvas
            var point = e.GetPosition(panel);

            if (point.X > panel.ActualWidth || point.X < 0 ||
                point.Y > panel.ActualHeight || point.Y < 0)
                return;

            //var btn = (Button) AssociatedObject;

            if (MainContentWindowViewModel.Instance.SketchType != typeof(T).Name)
            {
                MainContentWindowViewModel.Instance.Sketches = WIW.GetListOfWindowSketches<T>();
                MainContentWindowViewModel.Instance.SketchType = typeof(T).Name;
            }

            // Move the element.
            Canvas.SetLeft(MainContentWindow.Instance.SketchBorder, point.X - mouseOffset.X);

            //MainContentWindow.Instance.SketchBorder.SetValue(Canvas.LeftProperty, point.X - mouseOffset.X);
            //AssociatedObject.SetValue(Canvas.LeftProperty, point.X - mouseOffset.X);
        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                AssociatedObject.ReleaseMouseCapture();
                isDragging = false;
            }
        }


    }
    public class SketchBehavior : Behavior<UIElement>
    {
        private Type Type;

        public SketchBehavior(Type type)
        {
            this.Type = type;
        }

        //private Canvas canvas;

        private Panel panel;

        protected override void OnAttached()
        {
            base.OnAttached();

            // Присоединение обработчиков событий            
            //this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            panel ??= VisualTreeHelper.GetParent(this.AssociatedObject) as Panel;
            //this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            // Удаление обработчиков событий
            //this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            //this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
        }

        // Отслеживание перетаскивания элемента
        //private bool isDragging = false;

        // Запись точной позиции, в которой нажата кнопка
        private Point mouseOffset = new Point(28, 0);

        //private void AssociatedObject_MouseLeftButtonDown(object sender, MouseEventArgs e)
        //{
        //    // Поиск canvas
        //    //canvas ??= VisualTreeHelper.GetParent(this.AssociatedObject) as Canvas;

        //    // Режим перетаскивания
        //    isDragging = true;

        //    // Получение позиции нажатия относительно элемента
        //    mouseOffset = e.GetPosition(AssociatedObject);

        //    // Захват мыши
        //    AssociatedObject.CaptureMouse();
        //}

        public double MousePoint { get; set; }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (panel == null)
            {
                panel ??= VisualTreeHelper.GetParent(GetAncestorOfType<ListViewItem>(sender as Button)) as Panel;
                //panel ??= VisualTreeHelper.GetParent((DependencyObject)sender) as Panel;
                if(panel == null)
                    return;
            }
            // Получение позиции элемента относительно Canvas
            var point = e.GetPosition(panel);

            if (point.X > panel.ActualWidth || point.X < 0 ||
                point.Y > panel.ActualHeight || point.Y < 0)
            {
                if(MousePoint != default)
                    Canvas.SetLeft(MainContentWindow.Instance.SketchBorder, MousePoint);
                return;
            }
            
            //var btn = (Button) AssociatedObject;

            if (MainContentWindowViewModel.Instance.SketchType != Type.Name)
            {
                MainContentWindowViewModel.Instance.Sketches = WIW.GetListOfWindowSketches(Type);
                MainContentWindowViewModel.Instance.SketchType = Type.Name;
            }

            var result = point.X - (mouseOffset.X + MainContentWindow.Instance.SketchBorder.ActualWidth/2);

            MousePoint = result;


            // Move the element.
            Canvas.SetLeft(MainContentWindow.Instance.SketchBorder, result);

            //MainContentWindow.Instance.SketchBorder.SetValue(Canvas.LeftProperty, point.X - mouseOffset.X);
            //AssociatedObject.SetValue(Canvas.LeftProperty, point.X - mouseOffset.X);
        }

        public T GetAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T)) 
                return (T)GetAncestorOfType<T>((FrameworkElement)parent);
            return (T) parent;
        }

        //private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (isDragging)
        //    {
        //        AssociatedObject.ReleaseMouseCapture();
        //        isDragging = false;
        //    }
        //}
    }
}