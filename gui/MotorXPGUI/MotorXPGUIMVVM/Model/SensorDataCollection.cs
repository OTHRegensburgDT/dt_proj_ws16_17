using MotorXPGUIMVVM.Annotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MotorXPGUIMVVM.Model
{
    public sealed class SensorDataCollection : INotifyPropertyChanged
    {
        private const int MinSampleWindowValue = 10;
        private double _currentValue;
        private double _lastValue;
        private ulong _sampleWindow = MinSampleWindowValue;
        private bool _showAll = true;
        private int _targetValue;
        private int _trashValue;
        private BindingList<double> _values;
        private ulong _lastTimeStamp;

        public SensorDataCollection(SensorDataType type)
        {
            SensorDataType = type;
            Unit = type.ToString();
            InitSensorSettings();
        }


        public ulong LastTimeStamp
        {
            get { return _lastTimeStamp; }
            set
            {
                _lastTimeStamp = value <= 0 ? 0 : value;
                if (_showAll)
                {

                    if (MinSampleWindowValue < _lastTimeStamp)
                    {
                        SampleWindow = _lastTimeStamp;
                    }
                    if (MinSampleWindowValue > _lastTimeStamp)
                    {
                        SampleWindow = MinSampleWindowValue;
                    }
                }
                OnPropertyChanged(nameof(LastTimeStamp));
            }
        }

        public ulong SampleWindow
        {
            get { return _sampleWindow; }
            set
            {
                if ((_showAll || value >= LastTimeStamp) && LastTimeStamp >= MinSampleWindowValue)
                {
                    _sampleWindow = LastTimeStamp;
                }
                else if (value <= MinSampleWindowValue) // prevent to low sample windows
                {
                    _sampleWindow = MinSampleWindowValue;
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

        /// <summary>
        /// If true display complete SampleWindow
        /// </summary>
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

        public void AddValue(double value)
        {
            _values.Add(value);
            _lastValue = value;
            OnPropertyChanged(nameof(LastValue));
            OnPropertyChanged(nameof(Values));
        }

        /// <summary>
        /// all recived values should be a ringbuffer 
        /// </summary>
        public BindingList<double> Values
        {
            get { return _values; }
            private set
            {
                _values = value;
                LastValue = CheckMinMax(_values.LastOrDefault());
                if (_showAll)
                {
                    CurrentValue = LastValue;
                }
                OnPropertyChanged();
            }
        }

        public ICommand ShowAllCommand { get; set; }

        public ICommand SliderMouseButtonUpCommand { get; set; }

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
                    throw new InvalidEnumArgumentException();                    
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}