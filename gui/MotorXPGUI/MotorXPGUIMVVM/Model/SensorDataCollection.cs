using MotorXPGUIMVVM.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MotorXPGUIMVVM.Model
{
    public class SensorDataCollection :  INotifyPropertyChanged
    {
        private BindingList<double> _values;
        private double _lastValue = 0.0;
        private double _currentValue = 0.0;
        public SensorDataCollection(SensorDataType type)
        {
            SensorDataType = type;
            Values = new BindingList<double>();
        }

        public double CurrentValue
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                OnPropertyChanged(nameof(CurrentValue));
            }
        }


        public double LastValue
        {
            get { return _lastValue; }
            set
            {
                _lastValue = value;
                OnPropertyChanged(nameof(LastValue));
            }
        }
        
        public SensorDataType SensorDataType { get; } 

        public BindingList<double> Values
        {
            get { return _values; }
            private set
            {
                _values = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
