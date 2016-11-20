using System;
using System.Globalization;
using System.Windows.Data;

namespace MotorXPGUIMVVM.Converter {
    /// <summary>
    /// Returns half the value of a double.
    /// </summary>
    public class DoubleInHalfConverter : IValueConverter{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (double) value/2;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (double) value*2;
    }
}
