using System.ComponentModel;
using MotorXPGUIMVVM.Model;

namespace MotorXPGUIMVVM.Repository
{
    public interface ISensorRepository
    {
        BindingList<SensorDataCollection> SensorDataCollections { get; set; }
        event PropertyChangedEventHandler PropertyChanged;


    }
}