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

        /// <summary>
        /// ctor set the sensorDataType for a collection and run some init methods 
        /// </summary>
        /// <param name="type">sensordatatype</param>
        public SensorDataCollection(SensorDataType type)
        {
            SensorDataType = type;
            Unit = type.ToString();
            InitSensorSettings();
        }

        /// <summary>
        /// get or set the latest displayed timestamp  
        /// </summary>
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

        /// <summary>
        /// this is the current samplewindow which is displayed in the gui
        /// </summary>
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

        /// <summary>
        /// get the unit as string from a sensordata collection
        /// </summary>
        public string Unit { get; }

        /// <summary>
        /// get or set the current value from a sensorvalue, if show all is selected this should be everytime the last value from values
        /// </summary>
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

        /// <summary>
        /// check the min max bounds for a value
        /// </summary>
        /// <param name="value"> the incomming value</param>
        /// <returns>the value in bounds</returns>
        private double CheckMinMax(double value)
        {
            if (value <= MinValue)
            {
                return MinValue;
            }
            return value >= MaxValue ? MaxValue : value;
        }

        /// <summary>
        /// get the current value as text string
        /// </summary>
        public string CurrentValueText
            => $"Current Value: {(_showAll ? _lastValue.ToString("00") : _currentValue.ToString("00"))}";

        /// <summary>
        /// this property is only used for the velocety get get and set a target value
        /// </summary>
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

        /// <summary>
        /// get or set the last value from the value list and set the current value to the last if show all is selected
        /// </summary>
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

        /// <summary>
        /// this is the minimum gauge angle value to configure the radius from the gauge
        /// </summary>
        public double MinGaugeAngle { get; private set; }

        /// <summary>
        /// this is the maximum gauge angle value to configure the radius from the gauge
        /// </summary>
        public double MaxGaugeAngle { get; private set; }

        /// <summary>
        /// this is the minimum value which can be reached for a unit
        /// </summary>
        public int MinValue { get; private set; }

        /// <summary>
        /// this is the maximum value which can be reached for a unit
        /// </summary>
        public int MaxValue { get; private set; }

        /// <summary>
        /// the unit type of a data object
        /// </summary>
        public SensorDataType SensorDataType { get; }

        /// <summary>
        /// add a value to the value list and update the gui and the last value
        /// </summary>
        /// <param name="value">new value as double</param>
        public void AddValue(double value)
        {
            _values.Add(value);
            _lastValue = value;
            OnPropertyChanged(nameof(LastValue));
            OnPropertyChanged(nameof(Values));
        }

        /// <summary>
        /// all recived values 
        /// ToDo change to a ringbuffer and persist overflowing data  
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

        /// <summary>
        /// command triggers if showAll check changed
        /// </summary>
        public ICommand ShowAllCommand { get; set; }

        /// <summary>
        /// this slider start the send params method if the user release the mouse from the target slider
        /// </summary>
        public ICommand SliderMouseButtonUpCommand { get; set; }

        /// <summary>
        /// indicates if a sensordatacollection can have a target value, at the moment only the velocity possible
        /// </summary>
        public bool HasTargetValue { get; set; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// initialize the sensordatacollection for a specific sensordata type
        /// </summary>
        private void InitSensorSettings()
        {
            Values = new BindingList<double>();
            MinGaugeAngle = 4.65;
            MaxGaugeAngle = 7.85;
            //ToDo put settings like this in a config file
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
                    MaxGaugeAngle = 10.934;
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