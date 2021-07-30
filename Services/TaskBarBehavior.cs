using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using Q.ViewModels;
using Q.Views;

namespace Q.Services
{
    public class TaskBarBehavior : Behavior<UIElement>
    {
        private Canvas canvas;
        
        protected override void OnAttached()
        {
            base.OnAttached();                       

            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
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
        private Point mouseOffset;

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
            if ((VisualTreeHelper.GetParent(this.AssociatedObject) as Panel).Name != "MaximizePanel" ||
                !(this.AssociatedObject as ContentWindow).WindowIsFocused)
            {
                MainContentWindowViewModel.Instance.TaskBarIsHidden = false;
                return;
            }
            
            // Получение позиции элемента относительно Canvas
            var point = e.GetPosition(this.AssociatedObject);

            var widthLeft = MainContentWindow.Instance.ActualWidth / 2 - MainContentWindow.Instance.TaskBar.ActualWidth / 2;
            var widthRight = MainContentWindow.Instance.ActualWidth / 2 + MainContentWindow.Instance.TaskBar.ActualWidth / 2;

            if (point.X < widthLeft || point.X > widthRight) return;

            if (point.X > ((UserControl) this.AssociatedObject).ActualWidth || point.X < 0 ||
                point.Y > ((UserControl) this.AssociatedObject).ActualHeight || point.Y < 0 ||
                point.Y < ((UserControl) this.AssociatedObject).ActualHeight - 10)
            {
                if(MainContentWindowViewModel.Instance.TaskBarIsHidden != true)
                    MainContentWindowViewModel.Instance.TaskBarIsHidden = true;
                return;
            }
                
            MainContentWindowViewModel.Instance.TaskBarIsHidden = false;

            //// Move the element.
            //AssociatedObject.SetValue(Canvas.TopProperty, point.Y - mouseOffset.Y);
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
}