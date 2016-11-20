using System;
using System.Globalization;
using System.Windows.Data;

namespace MotorXPGUIMVVM.Converter {
    /// <summary>
    /// Converts a double to an integer.
    /// </summary>
    public class DoubleToIntConverter : IValueConverter{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => System.Convert.ToInt32((double) value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => System.Convert.ToDouble((int)value);
    }
}
