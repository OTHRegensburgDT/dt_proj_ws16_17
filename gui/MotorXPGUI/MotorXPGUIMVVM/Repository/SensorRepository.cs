using KomModule;
using Microsoft.Practices.Unity;
using MotorXPGUIMVVM.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MotorXPGUIMVVM.Repository
{

    public class SensorRepository : INotifyPropertyChanged, ISensorRepository
    {
        private readonly ICommunicator _com;

        private BindingList<SensorDataCollection> _sensorDataCollections;

        [InjectionConstructor]
        public SensorRepository()
        {

            _com = GetCommunicator();
            _com.NewSensordata += OnNewSensorData;
            HallSensorDataCollections = InitHallPattern();
            _sensorDataCollections = new BindingList<SensorDataCollection>();
        }

        private static ICommunicator GetCommunicator()
        {
            try
            {
               return new UartCommunicator("COM8");
            }
            catch (Exception)
            {
                
               return new MockCommunicator();
            }
        }

        private static BindingList<SensorDataCollection> InitHallPattern()
        {
            return new BindingList<SensorDataCollection>
           {
               new SensorDataCollection(SensorDataType.HallPattern),
               new SensorDataCollection(SensorDataType.HallPattern),
               new SensorDataCollection(SensorDataType.HallPattern),
           };
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

        public BindingList<SensorDataCollection> HallSensorDataCollections { get; }

        public double HallPatternWindowPosition { get; set; }

        public ICommand SubmitPIDCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        // ReSharper disable once InconsistentNaming
        public void SendPID(RegulationParams @params)
        {
            if (_com.SetParams(@params))
            {
                _com.SendParams();
            }
        }

        private void OnNewSensorData()
        {
            var dataTable = _com.GetData().DataTable;

            foreach (var data in dataTable)
            {
                var sensorDataCollection = SensorDataCollections.FirstOrDefault(x => x.SensorDataType == (SensorDataType)data.Key);
                if (sensorDataCollection == null 
                    && (SensorDataType)data.Key != SensorDataType.HallPattern)
                {
                    sensorDataCollection = new SensorDataCollection((SensorDataType)data.Key);
                    Application.Current.Dispatcher.Invoke(() => SensorDataCollections.Add(sensorDataCollection));

                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    sensorDataCollection?.AddValue(CheckMinValue(data.Value, sensorDataCollection));
                });

                // add hallpattern if angle value is arrived
                if ((SensorDataType)data.Key == SensorDataType.HallPattern)
                {
                    AddHallPattern(data.Value);
                }
            }
        }
        protected virtual double CheckMinValue(double dataValue, SensorDataCollection col)
        {
            return dataValue <= col.MinValue ? col.MinValue : dataValue;
        }
        private void AddHallPattern(double hallValue)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                HallSensorDataCollections[0].Values.Add((int)hallValue & (1<<0));
                HallSensorDataCollections[1].Values.Add((int)hallValue & (1 << 1));
                HallSensorDataCollections[2].Values.Add((int)hallValue & (1 << 2));
            });
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
