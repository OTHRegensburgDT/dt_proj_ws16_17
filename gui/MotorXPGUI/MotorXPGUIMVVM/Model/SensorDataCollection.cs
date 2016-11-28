using MotorXPGUIMVVM.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MotorXPGUIMVVM.Model
{
    public class SensorDataCollection : INotifyPropertyChanged
    {
        private BindingList<double> _values;
        private double _lastValue;
        private double _currentValue;
        private int _maxValue;
        private int _minValue;


        public SensorDataCollection(SensorDataType type)
        {
            SensorDataType = type;
            Init();
        }

        private void Init()
        {
            Values = new BindingList<double>();
            switch (SensorDataType)
            {
                case SensorDataType.Velocity:
                    _minValue = 0;
                    _maxValue = 6000;
                    break;
                case SensorDataType.Angle:
                    _minValue = -360;
                    _maxValue = 360;
                    break;
                case SensorDataType.Temp:
                    _minValue = 0;
                    _maxValue = 150;
                    break;
                case SensorDataType.HallPattern:
                    _minValue = 0;
                    _maxValue = 1;
                    break;
                default:
                    _minValue = 0;
                    _maxValue = 100;
                    break;
            }
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

        public int MinValue
        {
            get { return _minValue; }
        }


        public int MaxValue
        {
            get { return _maxValue; }
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
