using System;
using System.Globalization;
using System.Windows.Data;

namespace MotorXPGUIMVVM.Converter
{
    public class StringToIntConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => System.Convert.ToInt32((string) value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => System.Convert.ToString((int)value);
    }
}