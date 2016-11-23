using System;
using System.ComponentModel;
using MotorXPGUIMVVM.Model;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace MotorXPGUIMVVM.Repository
{
    public class MockSensorRepository : INotifyPropertyChanged, ISensorRepository
    {
        private BindingList<SensorDataCollection> _sensorDataCollections;

        private int _counter = 0;

        private Random rnd = new Random();

        public MockSensorRepository()
        {
            _sensorDataCollections = GetStartSensorData();
            StrartMockTask();
        }

        private void StrartMockTask()
        {
            Task.Run(() =>
            {
                {
                    while (true)
                    {
                        while (_counter++ < 500)
                        {
                            var j = _counter;
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                foreach (var col in _sensorDataCollections)
                                {
                                    var newValue = rnd.NextDouble();
                                    switch (col.SensorDataType)
                                    {
                                        case SensorDataType.Velocity:
                                            newValue *= 6000;                                            
                                            break;
                                        case SensorDataType.Angle:
                                            newValue *= 360;
                                            break;
                                        case SensorDataType.Temp:
                                            newValue *= 150;
                                            break;
                                        case SensorDataType.HallPattern:
                                            break;
                                        default:
                                            break;
                                    }
                                    col.Values.Add(newValue);
                                    col.LastValue = newValue;
                                }
                            });
                            Thread.Sleep(100);
                        }
                        while (_counter-- > 20)
                        {
                            var j = _counter;
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                foreach (var col in _sensorDataCollections)
                                {
                                    var newValue = rnd.NextDouble();
                                    switch (col.SensorDataType)
                                    {
                                        case SensorDataType.Velocity:
                                            newValue *= 6000;
                                            break;
                                        case SensorDataType.Angle:
                                            newValue *= 360;
                                            break;
                                        case SensorDataType.Temp:
                                            newValue *= 150;
                                            break;
                                        case SensorDataType.HallPattern:
                                            break;
                                        default:
                                            break;
                                    }
                                    col.Values.Add(newValue);
                                    col.LastValue = newValue;
                                }
                            });
                            Thread.Sleep(100);
                        }
                    }
                }
            });
        }

        private BindingList<SensorDataCollection> GetStartSensorData()
        {
            var dataCollection1 = new SensorDataCollection(SensorDataType.Velocity);
            var dataCollection2 = new SensorDataCollection(SensorDataType.Temp);
            var dataCollection3 = new SensorDataCollection(SensorDataType.Angle);

   

            for (int i = 0; i < 100; i++)
            {
                dataCollection1.Values.Add(rnd.NextDouble() * 6000.0);
                dataCollection2.Values.Add(rnd.NextDouble() * 150.0);
                dataCollection3.Values.Add(rnd.NextDouble() * 50.00);


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