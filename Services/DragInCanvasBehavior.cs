using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace Q.Services
{
    public class DragInCanvasBehavior : Behavior<UIElement>
    {
        private Canvas canvas;
        
        protected override void OnAttached()
        {
            base.OnAttached();                       

            // Присоединение обработчиков событий            
            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            // Удаление обработчиков событий
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
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
            if (!isDragging || canvas == null) return;
            // Получение позиции элемента относительно Canvas
            var point = e.GetPosition(canvas);

            if(point.X > canvas.ActualWidth || point.X < 0 ||
               point.Y > canvas.ActualHeight || point.Y < 0)
                return;

            // Move the element.
            AssociatedObject.SetValue(Canvas.TopProperty, point.Y - mouseOffset.Y);
            AssociatedObject.SetValue(Canvas.LeftProperty, point.X - mouseOffset.X);
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