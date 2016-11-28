using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MotorXPGUIMVVM.Converter
{
    /// <summary>
    /// Converts a boolean to Visibility. With false = hidden.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class BooleanToVisibilityHiddenConverter : IValueConverter{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool) value ? Visibility.Visible : Visibility.Hidden;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (Visibility) value == Visibility.Visible;
    }
}
