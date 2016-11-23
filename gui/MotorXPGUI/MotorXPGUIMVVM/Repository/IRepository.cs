using System;

namespace MotorXPGUIMVVM.Repository
{
    public interface IRepository
    {
        /// <summary>
        /// get torque in nm as double
        /// </summary>
        /// <returns>double value</returns>
        ulong GetElectroMagneticTorque();
       
        /// <summary>
        /// Get hall effect true or false
        /// </summary>
        /// <returns>boolean value</returns>
        bool GetHallEffect();

        /// <summary>
        /// get rotor speed in rad/sec
        /// </summary>
        /// <returns>double value</returns>
        ulong GetRotorSpeed();

        /// <summary>
        /// get current stator voltage (V) as double
        /// </summary>
        /// <returns>double value</returns>
        ulong GetStatorCurrent();

        /// <summary>
        /// Get timestamp from object
        /// </summary>
        /// <returns>datetime value</returns>
        UInt64 GetTimeStamp();

        /// <summary>
        /// Triggers 
        /// </summary>
        /// <returns></returns>
        Action NewValueArrived();
    }
}