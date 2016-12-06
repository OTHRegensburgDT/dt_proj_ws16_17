using MotorXPGUIMVVM.Model;
using System.ComponentModel;
using System.Windows.Input;

namespace MotorXPGUIMVVM.Repository
{
    public interface ISensorRepository
    {
        BindingList<SensorDataCollection> SensorDataCollections { get; set; }
        ICommand SubmitPIDCommand { get; set; }
        event PropertyChangedEventHandler PropertyChanged;


        // ReSharper disable once InconsistentNaming
        void SendPID();
    }
}