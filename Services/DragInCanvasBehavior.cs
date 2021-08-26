using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualBasic;
using Microsoft.Xaml.Behaviors;
using Q.Views;
using QCore;

namespace Q.Services
{
    public class DragInCanvasBehavior : Behavior<UIElement>
    {
        private Canvas canvas;

        private bool _isStretching;
        
        protected override void OnAttached()
        {
            base.OnAttached();                       

            // Присоединение обработчиков событий            
            ((UserControl)this.AssociatedObject).MouseDoubleClick += OnMouseDoubleClick;
            this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeave += AssociatedObjectOnMouseLeave;
            ((ContentWindow) AssociatedObject).Loaded += OnLoaded;
            this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        private void AssociatedObjectOnMouseLeave(object sender, MouseEventArgs e)
        {
            ChangeCursor(Cursors.Arrow);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ((ContentWindow) AssociatedObject).MainBorder.MouseLeave += BorderOnMouseLeave;
        }

        private void BorderOnMouseLeave(object sender, MouseEventArgs e)
        {
            var border = ((ContentWindow) AssociatedObject).MainBorder;
            var rightLimit = border.ActualWidth - border.Padding.Right;
            var bottomLimit = border.ActualHeight - border.Padding.Bottom;
            var point = Mouse.GetPosition((Border)sender);

            // figure out stretching directions - only to Right, Bottom 
            var stretchRight = point.X >= rightLimit && point.X < ((ContentWindow) AssociatedObject).ActualWidth;
            var stretchBottom = point.Y >= bottomLimit && point.Y < ((ContentWindow) AssociatedObject).ActualHeight;

            //update element
            //Canvas.SetLeft(AssociatedObject, point.X);
            //Canvas.SetTop( AssociatedObject, point.Y);
            this._isStretching = true;

            if (stretchRight && stretchBottom)
            {
                ChangeCursor(Cursors.SizeNWSE);
                return;
            }

            if (stretchRight && !stretchBottom)
            {
                ChangeCursor(Cursors.SizeWE);
                return;
            }

            if (!stretchRight && stretchBottom)
            {
                ChangeCursor(Cursors.SizeNS);
                return;
            }
            
            ChangeCursor(Cursors.Arrow);
            this._isStretching = false;
        }

        private void ChangeCursor(Cursor cursor)
        {
            ((ContentWindow) AssociatedObject).Cursor = cursor;
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WIW.Maximize(((ContentWindow)this.AssociatedObject).ContentControl.Content);
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
            // Получение позиции элемента относительно Canvas
            
            var point = e.GetPosition(canvas);

            var left = Canvas.GetLeft(AssociatedObject);
            var top = Canvas.GetTop(AssociatedObject);

            
            var width = ((ContentWindow) AssociatedObject).ActualWidth;
            var minWidth =  ((ContentWindow) AssociatedObject).MinWidth;
            var height = ((ContentWindow) AssociatedObject).ActualHeight;
            var minHeight =  ((ContentWindow) AssociatedObject).MinHeight;

            var xDiff = point.X - (left + width);
            var yDiff = point.Y - (top + height);

            //make sure not to resize to negative width or heigth
            xDiff = (width + xDiff) > minWidth ? xDiff : minWidth;
            yDiff = (height + yDiff) > minHeight ? yDiff : minHeight;

            // stretchRight && stretchBottom ?
            var cursor = ((ContentWindow) AssociatedObject).Cursor;

            if (cursor == Cursors.SizeNWSE)
            {
                ((ContentWindow) AssociatedObject).Width += xDiff;
                ((ContentWindow) AssociatedObject).Height += yDiff;
            }
            else if (cursor == Cursors.SizeWE)
            {
                if (!isDragging || canvas == null || ((ContentWindow) AssociatedObject).Width <= minWidth) return;
                ((ContentWindow) AssociatedObject).Width += xDiff * 0.2;
                ((ContentWindow) AssociatedObject).Test1.Text = xDiff.ToString();
            }
            else if (cursor == Cursors.SizeNS)
                ((ContentWindow) AssociatedObject).Height += yDiff;
            else
            {
                ((ContentWindow) AssociatedObject).Cursor = Cursors.Arrow;
            }

            if (!isDragging || canvas == null || _isStretching) return;

            var canvasPos = e.GetPosition(canvas);

            if(canvasPos.X > canvas.ActualWidth || canvasPos.X < 0 ||
               canvasPos.Y > canvas.ActualHeight || canvasPos.Y < 0)
                return;

            var xmouse = AssociatedObject.GetValue(Canvas.LeftProperty);
            var ymouse = AssociatedObject.GetValue(Canvas.TopProperty);

            // Move the element.
            AssociatedObject.SetValue(Canvas.TopProperty, canvasPos.Y - mouseOffset.Y);
            AssociatedObject.SetValue(Canvas.LeftProperty, canvasPos.X - mouseOffset.X);

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