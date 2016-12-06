using MotorXPGUIMVVM.Model;
using System.ComponentModel;
using System.Windows.Input;
using KomModule;

namespace MotorXPGUIMVVM.Repository
{
    public interface ISensorRepository
    {
        BindingList<SensorDataCollection> SensorDataCollections { get; set; }
        
        // ReSharper disable once InconsistentNaming
        ICommand SubmitPIDCommand { get; set; }
        event PropertyChangedEventHandler PropertyChanged;

        // ReSharper disable once InconsistentNaming
        void SendPID(RegulationParams @params);
    }
}