using MotorXPGUIMVVM.Model;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MotorXPGUIMVVM.Repository
{
    public class MockSensorRepository : INotifyPropertyChanged, ISensorRepository
    {
        private BindingList<SensorDataCollection> _sensorDataCollections;

        private ulong _counter;

        private readonly Random _rnd = new Random();

        public MockSensorRepository()
        {
            _sensorDataCollections = GetStartSensorData();
            StrartMockTask();
        }

        private void StrartMockTask()
        {
            Task.Run(() =>
            {
                while (true)
                {

                    if (Application.Current != null)
                    {
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
                                        newValue = _rnd.Next(-360, 360);
                                        break;
                                    case SensorDataType.Temp:
                                        newValue = _rnd.Next(0, 150);
                                        break;
                                    case SensorDataType.HallPattern:
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                col.Values.Add(newValue);
                                col.LastValue = newValue;
                                col.SampleList.Add(_counter++);
                            }
                        });
                        Thread.Sleep(300);
                    }
                }
                // ReSharper disable once FunctionNeverReturns
            });
        }

        private BindingList<SensorDataCollection> GetStartSensorData()
        {
            var dataCollection1 = new SensorDataCollection(SensorDataType.Velocity);
            var dataCollection2 = new SensorDataCollection(SensorDataType.Temp);
            var dataCollection3 = new SensorDataCollection(SensorDataType.Angle);

            for (var i = 0; i < 100; i++)
            {
                dataCollection1.Values.Add(_rnd.NextDouble() * 6000.0);
                dataCollection2.Values.Add(_rnd.NextDouble() * 150.0);
                dataCollection3.Values.Add(_rnd.NextDouble() * 50.00);
            }

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}