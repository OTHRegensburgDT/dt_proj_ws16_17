using System;
using System.ComponentModel;
using MotorXPGUIMVVM.Model;

namespace MotorXPGUIMVVM.Repository
{
    public class MockSensorRepository : INotifyPropertyChanged, ISensorRepository
    {
        private BindingList<SensorDataCollection> _sensorDataCollections;


        public MockSensorRepository()
        {
            _sensorDataCollections = GetSensorData();
        

        }

        private BindingList<SensorDataCollection> GetSensorData()
        {
            var dataCollection1 = new SensorDataCollection(SensorDataType.Velocity);
            var dataCollection2 = new SensorDataCollection(SensorDataType.Temp);
            var dataCollection3 = new SensorDataCollection(SensorDataType.Angle);

            var rnd = new Random();

            for (int i = 10; i < 510; i++)
            {
                dataCollection1.Values.Add(rnd.NextDouble() * 500.0);
            }
            for (int i = 10; i < 400; i++)
            {
                dataCollection2.Values.Add(rnd.NextDouble() * 500.0);
            }
            for (int i = 10; i < 600; i++)
            {
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