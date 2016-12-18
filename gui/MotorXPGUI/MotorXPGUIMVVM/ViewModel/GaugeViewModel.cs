using GalaSoft.MvvmLight;
using System;
using System.Globalization;

namespace MotorXPGUIMVVM.ViewModel
{
    public class GaugeViewModel : ViewModelBase
    {       
        private double _gaugeValue;


        public string Unit { get; set; } = "rpm";
        public string Title { get; set; } = "Demo";
        public double GaugeValue
        {
            get
            {
                // ToDo put into function and make it better :D 
    
                double targetRange = (Math.Abs(MaxValue) - Math.Abs(MinValue));
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                double targetNormalized = targetRange == 0.0 ? 1.0 : targetRange;
                double retValue = -120 + 240 * (_gaugeValue / targetNormalized);

                return retValue;
            }
            set
            {
                _gaugeValue = value;
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(GaugeValue));
            }
        }

        public string GaugValueText
        {
            get { return _gaugeValue.ToString(CultureInfo.InvariantCulture); }
        }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }
}