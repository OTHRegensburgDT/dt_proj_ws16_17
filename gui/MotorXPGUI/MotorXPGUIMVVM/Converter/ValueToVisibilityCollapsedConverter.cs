using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MotorXPGUIMVVM.Converter
{
    /// <summary>
    /// Converts a boolean to Visibility. With false = collapsed.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class ValueToVisibilityCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (Visibility)value == Visibility.Visible;
    }

}