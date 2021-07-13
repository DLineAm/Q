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

        public static readonly DependencyProperty MultipleWindowsProperty = DependencyProperty.RegisterAttached(
            "MultipleWindows", typeof(bool), typeof(UIExtensions), new PropertyMetadata(true));

        public static void SetMultipleWindows(DependencyObject element, bool value)
        {
            element.SetValue(MultipleWindowsProperty, value);
        }

        public static bool GetMultipleWindows(DependencyObject element)
        {
            return (bool)element.GetValue(MultipleWindowsProperty);
        }
    }
}