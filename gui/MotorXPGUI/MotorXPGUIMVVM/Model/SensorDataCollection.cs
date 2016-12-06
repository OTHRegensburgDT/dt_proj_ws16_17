using MotorXPGUIMVVM.Annotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MotorXPGUIMVVM.Model
{
    public class SensorDataCollection : INotifyPropertyChanged
    {
        private BindingList<double> _values;
        private double _lastValue;
        private double _currentValue;
        private bool _showAll = true;
        private bool _expanderCollapsed;
        private int _targetValue;
        private int _trashValue;
        private ulong _currentSample;
        private ulong _sampleWindow = 10;

        public SensorDataCollection(SensorDataType type)
        {
            SensorDataType = type;
            Unit = type.ToString();
            InitSensorSettings();

        }

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
                    MinValue = -360;
                    MaxValue = 360;
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
                if (value > LastTimeStamp)
                {
                    _sampleWindow = LastTimeStamp;
                }
                else if (value <= 10)
                {
                    _sampleWindow = 10;
                }
                _sampleWindow = value;
                OnPropertyChanged(nameof(SampleWindow));
            }
        }

        public string Unit { get; }

        public bool ExpanderCollapsed
        {
            get { return _expanderCollapsed; }
            set
            {
                _expanderCollapsed = value;
                OnPropertyChanged(nameof(ExpanderCollapsed));
            }
        }

        public double CurrentValue
        {
            get
            {
                return _showAll ? _lastValue : _currentValue;
            }
            set
            {
                _currentValue = value;
                OnPropertyChanged(nameof(CurrentValue));
                OnPropertyChanged(nameof(CurrentValueText));
            }
        }

        public string CurrentValueText => $"Current Value: {(_showAll ? _lastValue.ToString("00") : _currentValue.ToString("00"))}";

        public int TargetValue
        {
            get { return _targetValue; }
            set
            {
                var newValue = value;
                if (int.TryParse(newValue.ToString(), out _trashValue))
                {
                    if (newValue >= MinValue && newValue <= MaxValue)
                    {

                        _targetValue = newValue;
                        OnPropertyChanged(nameof(TargetValue));
                    }
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
                    SampleWindow = _lastTimeStamp;
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
                _values = value;
                LastValue = _values.LastOrDefault();
                OnPropertyChanged();
            }
        }

        public ICommand ShowAllCommand { get; set; }

        public int GaugeTickFrequency => MaxValue/10;

        public bool HasTargetValue { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
