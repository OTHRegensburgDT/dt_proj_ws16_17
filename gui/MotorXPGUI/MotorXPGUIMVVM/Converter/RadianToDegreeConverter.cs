using System;
using System.Globalization;
using System.Windows.Data;

namespace MotorXPGUIMVVM.Converter
{
    /// <summary>
    /// Converts radian to degree and subtracts 90 degree.
    /// </summary>
    public class RadianToDegreeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (double) value/Math.PI*180-90;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => ((double)value+90) / 180 / Math.PI;
    }
}
