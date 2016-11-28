using MotorXPGUIMVVM.Model;
using System.ComponentModel;

namespace MotorXPGUIMVVM.Repository
{
    public interface ISensorRepository
    {
        BindingList<SensorDataCollection> SensorDataCollections { get; set; }
        event PropertyChangedEventHandler PropertyChanged;


    }
}