using MotorXPGUIMVVM.Model;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KomModule;

namespace MotorXPGUIMVVM.Repository
{
    public class MockSensorRepository : INotifyPropertyChanged, ISensorRepository
    {
        private BindingList<SensorDataCollection> _sensorDataCollections;
        private readonly Random _rnd = new Random();

        public MockSensorRepository()
        {
            _sensorDataCollections = GetStartSensorData();
            StrartMockTask();
        }

        private void StrartMockTask()
        {
            var task =  Task.Run(() =>
                        {
                            while (true)
                            {
                                if (Application.Current == null) continue;
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    foreach (var col in _sensorDataCollections)
                                    {
                                        var newValue = 0;
                                        switch (col.SensorDataType)
                                        {
                                            case SensorDataType.Velocity:
                                                newValue = _rnd.Next(col.TargetValue - 100, col.TargetValue + 100);          
                                                break;
                                            case SensorDataType.Angle:
                                                newValue = _rnd.Next(-43, 43);
                                                break;
                                            case SensorDataType.Temp:
                                                newValue = _rnd.Next(36, 46);
                                                break;
                                            case SensorDataType.HallPattern:
                                                break;
                                            default:
                                                throw new ArgumentOutOfRangeException();
                                        }
                                        col.Values.Add(newValue);
                                        col.LastValue = newValue;
                                        col.LastTimeStamp++;
                                    }
                                });
                                Thread.Sleep(300);
                            }
                            // ReSharper disable once FunctionNeverReturns
                        });            
        }

        private BindingList<SensorDataCollection> GetStartSensorData()
        {
            var dataCollection1 = new SensorDataCollection(SensorDataType.Velocity);
            var dataCollection2 = new SensorDataCollection(SensorDataType.Temp);
            var dataCollection3 = new SensorDataCollection(SensorDataType.Angle);

            //for (var i = 0; i < 100; i++)
            //{
            //    dataCollection1.Values.Add(_rnd.NextDouble() * 6000.0);
            //    dataCollection2.Values.Add(_rnd.NextDouble() * 150.0);
            //    dataCollection3.Values.Add(_rnd.NextDouble() * 50.00);
            //}

            return new BindingList<SensorDataCollection> { dataCollection1, dataCollection2, dataCollection3 };
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

        public ICommand SubmitPIDCommand { get; set; }

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