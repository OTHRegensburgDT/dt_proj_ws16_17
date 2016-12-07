using System;
using KomModule;
using MotorXPGUIMVVM.Model;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MotorXPGUIMVVM.Repository
{
    public class SensorRepository : INotifyPropertyChanged, ISensorRepository
    {
        private readonly ICommunicator _com;

        private BindingList<SensorDataCollection> _sensorDataCollections;
        private readonly BindingList<SensorDataCollection> _hallSensorDataCollections;

        public SensorRepository(ICommunicator com)
        {
            _com = com;
            _com.NewSensordata += OnNewSensorData;
            _hallSensorDataCollections = InitHallPattern();
            _sensorDataCollections = new BindingList<SensorDataCollection>();
        }

        private BindingList<SensorDataCollection> InitHallPattern()
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

        public BindingList<SensorDataCollection> HallSensorDataCollections => _hallSensorDataCollections;

        public ICommand SubmitPIDCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        // ReSharper disable once InconsistentNaming
        public void SendPID(RegulationParams @params)
        {
            if (_com.SetParams(@params)) _com.SendParams();
        }

        private void OnNewSensorData()
        {
            var dataTable = _com.GetData().DataTable;

            foreach (var data in dataTable)
            {
                var sensorDataCollection = SensorDataCollections.FirstOrDefault(x => x.SensorDataType == (SensorDataType)data.Key);
                if ( sensorDataCollection == null)
                {
                    sensorDataCollection = new SensorDataCollection((SensorDataType)data.Key);
                    SensorDataCollections.Add(sensorDataCollection);
                }
                sensorDataCollection.Values.Add(data.Value);
                sensorDataCollection.LastValue = data.Value;
                // add hallpattern if angle value is arrived
                if ((SensorDataType) data.Key == SensorDataType.Angle)
                {
                    AddHallPattern(data.Value);
                }
            }
        }

        private void AddHallPattern(double angle)
        {
            _hallSensorDataCollections[0].Values.Add(GetHallA(angle));
            _hallSensorDataCollections[1].Values.Add(GetHallB(angle));
            _hallSensorDataCollections[2].Values.Add(GetHallC(angle));
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
      
    }
}
