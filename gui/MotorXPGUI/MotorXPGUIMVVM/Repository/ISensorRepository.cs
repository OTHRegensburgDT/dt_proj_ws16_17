using KomModule;
using MotorXPGUIMVVM.Model;
using System.ComponentModel;
using System.Windows.Input;

namespace MotorXPGUIMVVM.Repository
{
    public interface ISensorRepository
    {
        BindingList<SensorDataCollection> SensorDataCollections { get; set; }
        BindingList<SensorDataCollection> HallSensorDataCollections { get; }

        double HallPatternWindowPosition { get; set; }

        // ReSharper disable once InconsistentNaming
        ICommand SubmitPIDCommand { get; set; }
        event PropertyChangedEventHandler PropertyChanged;

        // ReSharper disable once InconsistentNaming
        void SendPID(RegulationParams @params);
    }
}