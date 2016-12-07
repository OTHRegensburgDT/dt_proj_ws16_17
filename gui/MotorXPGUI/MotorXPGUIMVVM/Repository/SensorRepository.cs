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

        public SensorRepository(ICommunicator com)
        {
            _com = com;
            _com.NewSensordata += OnNewSensorData;
            _sensorDataCollections = new BindingList<SensorDataCollection>();
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
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
      
    }
}
