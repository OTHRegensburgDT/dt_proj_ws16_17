using System;
namespace KomModule
{
    public interface ICommunicator
    {
        //getter für aktuelles Sensordatenobjekt
        Sensordata getData();
        //event neue Daten liegen vor
        event Action newSensordata;
        //start sending regulation parameters
        bool SendParams();
        //provide regulation parameters
        bool SetParams(RegulationParams para);
        //status Flag if Com port is open
        bool isInitialized();
    }
}
