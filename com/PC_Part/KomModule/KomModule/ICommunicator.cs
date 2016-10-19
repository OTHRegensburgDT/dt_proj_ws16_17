using System;
namespace KomModule
{
    public interface ICommunicator
    {
        Sensordata getData();
        event Action newSensordata;
        bool SendParams();
    }
}
