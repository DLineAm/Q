using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace Q.Services
{
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
    where T : class, new()
    {
        private static T mConverter = null;

        public override object ProvideValue(IServiceProvider serviceProvider) => mConverter ??= new T();

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
