using MotorXPGUIMVVM.Annotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MotorXPGUIMVVM.Model
{
    public sealed class SensorDataCollection : INotifyPropertyChanged
    {
        private ulong _currentSample;
        private double _currentValue;
        private double _lastValue;
        private ulong _sampleWindow = 10;
        private bool _showAll = true;
        private int _targetValue;
        private int _trashValue;
        private BindingList<double> _values;

        public SensorDataCollection(SensorDataType type)
        {
            SensorDataType = type;
            Unit = type.ToString();
            InitSensorSettings();
        }


        public ulong LastTimeStamp { get; set; }

        public ulong CurrentSample
        {
            get { return _currentSample; }
            set
            {
                _currentSample = value;
                OnPropertyChanged(nameof(CurrentSample));
            }
        }

        public ulong SampleWindow
        {
            get { return _sampleWindow; }
            set
            {
                if (value > LastTimeStamp) // max sample window 
                {
                    _sampleWindow = LastTimeStamp; 
                }
                else if (value <= 10) // prevent to low sample windows
                {
                    _sampleWindow = 10;
                }
                else
                {
                    _sampleWindow = value;
                }
                OnPropertyChanged(nameof(SampleWindow));
            }
        }

        public string Unit { get; }

        public double CurrentValue
        {
            get { return _showAll ? _lastValue : _currentValue; }
            set
            {
                _currentValue = CheckMinMax(value);                
                OnPropertyChanged(nameof(CurrentValue));
                OnPropertyChanged(nameof(CurrentValueText));
            }
        }

        private double CheckMinMax(double value)
        {
            if (value <= MinValue)
            {
                return MinValue;
            }
            if (value >= MaxValue)
            {
                return MaxValue;
            }
            return value;
        }

        public string CurrentValueText
            => $"Current Value: {(_showAll ? _lastValue.ToString("00") : _currentValue.ToString("00"))}";

        public int TargetValue
        {
            get { return _targetValue; }
            set
            {
                var newValue = value;
                if (int.TryParse(newValue.ToString(), out _trashValue))
                {                 
                    _targetValue = (int)CheckMinMax(value);
                    OnPropertyChanged(nameof(TargetValue));
                }
                else
                {
                    OnPropertyChanged(nameof(TargetValue));
                }
            }
        }

        public bool ShowAll
        {
            get { return _showAll; }
            set
            {
                _showAll = value;
                if (_showAll)
                {
                    SampleWindow = LastTimeStamp;
                }
                OnPropertyChanged(nameof(ShowAll));
            }
        }

        public double LastValue
        {
            get { return _lastValue; }
            set
            {
                if (_showAll)
                {
                    _currentValue = value;
                }
                _lastValue = value;
                OnPropertyChanged(nameof(LastValue));
            }
        }

        public int MinValue { get; private set; }

        public int MaxValue { get; private set; }

        public SensorDataType SensorDataType { get; }

        public BindingList<double> Values
        {
            get { return _values; }
            private set
            {
                _values =  value;
                LastValue = CheckMinMax(_values.LastOrDefault());
                if (_showAll)
                {
                    CurrentValue = LastValue;
                }
                OnPropertyChanged();
            }
        }

        public ICommand ShowAllCommand { get; set; }

        public int GaugeTickFrequency => MaxValue / 10;

        public bool HasTargetValue { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InitSensorSettings()
        {
            Values = new BindingList<double>();
            switch (SensorDataType)
            {
                case SensorDataType.Velocity:
                    MinValue = 0;
                    MaxValue = 6000;
                    TargetValue = 1500;
                    HasTargetValue = true;
                    break;
                case SensorDataType.Angle:
                    MinValue = 0;
                    MaxValue = 359;
                    break;
                case SensorDataType.Temp:
                    MinValue = 0;
                    MaxValue = 150;
                    break;
                case SensorDataType.HallPattern:
                    MinValue = 0;
                    MaxValue = 1;
                    break;
                default:
                    MinValue = 0;
                    MaxValue = 100;
                    TargetValue = 42;
                    break;
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}