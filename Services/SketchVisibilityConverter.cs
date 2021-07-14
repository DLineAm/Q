using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Q.Services
{
    public class SketchVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var hasFocusTaskBar = (bool)values[0];
                var hasFocusSketchBorder = (bool)values[1];
                var listCount = (int)values[2];

                if ((hasFocusSketchBorder || hasFocusTaskBar) && listCount != 0) return Visibility.Visible;
                return Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}