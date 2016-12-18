using KomModule;
using MotorXPGUIMVVM.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MotorXPGUIMVVM.Repository
{
    public class MockSensorRepository : INotifyPropertyChanged, ISensorRepository
    {
        private BindingList<SensorDataCollection> _sensorDataCollections;
        private BindingList<SensorDataCollection> _hallDataCollections;
        private readonly Random _rnd = new Random();

        public MockSensorRepository()
        {
            InitHallCollections();
            GetStartSensorData();        
            StrartMockTask();
        }

        private void InitHallCollections()
        {
            _hallDataCollections = new BindingList<SensorDataCollection>();
            for (int i = 0; i < 3; i++)
            {
                _hallDataCollections.Add(new SensorDataCollection(SensorDataType.HallPattern));
            }
        }

        private void StrartMockTask()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (Application.Current == null) continue;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var col in _sensorDataCollections)
                        {
                            int newValue;
                            switch (col.SensorDataType)
                            {
                                case SensorDataType.Velocity:
                                    newValue = _rnd.Next(col.TargetValue - 100 <= 0 ?  0 : col.TargetValue - 100, col.TargetValue + 100 >= 6000 ?  6000 : col.TargetValue + 100);          
                                    break;
                                case SensorDataType.Angle:
                                    newValue =  (int)(col.LastValue +1)%360;
                                    AddHallPattern();
                                    break;
                                case SensorDataType.Temp:
                                    newValue = _rnd.Next(36, 46);
                                    break;                               
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            col.AddValue(newValue);
                            col.LastTimeStamp++;
                        }
                    });
                    Thread.Sleep(100);
                }
                // ReSharper disable once FunctionNeverReturns
            });
        }

        private void AddHallPattern()
        {   
            _hallDataCollections[0].Values.Add(GetHallA(SensorDataCollections[2].Values.Last()));
            _hallDataCollections[1].Values.Add(GetHallB(SensorDataCollections[2].Values.Last()));
            _hallDataCollections[2].Values.Add(GetHallC(SensorDataCollections[2].Values.Last()));            
        }

        private void GetStartSensorData()
        {
            var velocityCol = new SensorDataCollection(SensorDataType.Velocity);
            var tempCol = new SensorDataCollection(SensorDataType.Temp);
            var angleCol = new SensorDataCollection(SensorDataType.Angle);
            for (var i = 0; i < 100; i++)
            {
                velocityCol.Values.Add(_rnd.NextDouble() * 6000.0);
                tempCol.Values.Add(_rnd.NextDouble() * 43.0);
                if (angleCol.Values.Count == 0)
                {
                    angleCol.Values.Add(0);
                }
                var angleValueToAdd = (angleCol.Values.Last()+1)%360;
                angleCol.Values.Add(angleValueToAdd);
                _hallDataCollections[0].Values.Add(GetHallA(angleCol.Values.Last()));
                _hallDataCollections[1].Values.Add(GetHallB(angleCol.Values.Last()));
                _hallDataCollections[2].Values.Add(GetHallC(angleCol.Values.Last()));
            }
            _sensorDataCollections =  new BindingList<SensorDataCollection> { velocityCol, tempCol, angleCol };
        }
        private static double GetHallA(double angle)
        {
            var preAngle = PrepareAngle(angle);
            if (preAngle >= -60.0 && preAngle <= 120) return 1.0;
            return 0;
        }
        private static double GetHallB(double angle)
        {
            var preAngle = PrepareAngle(angle);
            if (preAngle >= 60.0 || preAngle <= -120) return 1.0;
            return 0;
        }
        private static double GetHallC(double angle)
        {
            var preAngle = PrepareAngle(angle);
            if (preAngle >= -180.0 && preAngle <= 0) return 1.0;
            return 0;
        }
        private static double PrepareAngle(double angle)
        {
            var sin = Math.Sin(angle);
            var cos = Math.Cos(angle);
            var aTan2 = Math.Atan2(sin, cos);
            return aTan2 * (180 / Math.PI);
        }
        public BindingList<SensorDataCollection> SensorDataCollections
        {
            get { return _sensorDataCollections; }
            set
            {
                _sensorDataCollections = value;
                OnPropertyChanged(nameof(SensorDataCollections));
            }
        }
        public BindingList<SensorDataCollection> HallSensorDataCollections => _hallDataCollections;
        public ICommand SubmitPIDCommand { get; set; }

        private double _hallPatternWindowPosition;

        public double HallPatternWindowPosition
        {
            get { return _hallPatternWindowPosition; }
            set
            {
                _hallPatternWindowPosition = value; 

                OnPropertyChanged(nameof(HallPatternWindowPosition));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void SendPID( RegulationParams @params)
        {

        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}