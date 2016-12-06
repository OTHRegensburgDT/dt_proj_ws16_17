using KomModule;
using MotorXPGUIMVVM.Model;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MotorXPGUIMVVM.Repository
{
    public class SensorRepository : INotifyPropertyChanged, ISensorRepository
    {
        private ICommunicator _com;

        private BindingList<SensorDataCollection> _sensorDataCollections;

        public SensorRepository(ICommunicator com)
        {
            _com = com;
            _com.newSensordata += OnNewSensorData;
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
        public void SendPID()
        {
            throw new System.NotImplementedException();
        }

        private void OnNewSensorData()
        {
            var dataTable = _com.getData().DataTable;

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
