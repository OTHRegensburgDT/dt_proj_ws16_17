using System;

namespace MotorXPGUIMVVM.Repository
{
    public interface IRepository
    {
        /// <summary>
        /// get torque in nm as double
        /// </summary>
        /// <returns>double value</returns>
        double GetElectroMagneticTorque();
       
        /// <summary>
        /// Get hall effect true or false
        /// </summary>
        /// <returns>boolean value</returns>
        bool GetHallEffect();
        
        /// <summary>
        /// get rotor speed in rad/sec
        /// </summary>
        /// <returns>double value</returns>
        double GetRotorSpeed();
        
        /// <summary>
        /// get current stator voltage (V) as double
        /// </summary>
        /// <returns>double value</returns>
        double GetStatorCurrent();

        /// <summary>
        /// Get timestamp from object
        /// </summary>
        /// <returns>datetime value</returns>
        DateTime GetTimeStamp();

        /// <summary>
        /// Triggers 
        /// </summary>
        /// <returns></returns>
        Action NewValueArrived();
    }
}