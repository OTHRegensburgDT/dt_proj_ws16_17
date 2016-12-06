using System;
namespace KomModule
{
    public interface ICommunicator
    {
        /*
         * getData
         * return: received Sensordata object
         * description: method returns the received sensordata object
         */
        Sensordata getData();

        /*
         * newSensordata
         * description: event to be thrown when new Sensordata object
         * is available
         */
        event Action newSensordata;

        /*
         * SendParams
         * return: boolean value if send was successful
         * description: start sending of regulation parameters
         *      SetParams should be called first
         */
        bool SendParams();

        /*
         * SetParams
         * parameters: RegulationParam object with new parameters for motor regulation
         * return: boolean value if parameters were set successfully
         * description: set regulation parameters before send is issued
         */
        bool SetParams(RegulationParams para);

        /*
         * isInitialized
         * return: boolean value if Communication was initialized
         * description: is communication initialized? (means: com port open)
         */
        bool isInitialized();
    }
}
