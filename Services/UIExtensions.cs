using System.Windows;

namespace Q.Services
{
    public static class UIExtensions
    {
        public static readonly DependencyProperty CustomTitleProperty = DependencyProperty.RegisterAttached(
            "CustomTitle", typeof(string), typeof(UIExtensions), new PropertyMetadata(""));

        public static void SetCustomTitle(DependencyObject element, string value)
        {
            element.SetValue(CustomTitleProperty, value);
        }

        public static string GetCustomTitle(DependencyObject element)
        {
            return (string)element.GetValue(CustomTitleProperty);
        }
    }
}