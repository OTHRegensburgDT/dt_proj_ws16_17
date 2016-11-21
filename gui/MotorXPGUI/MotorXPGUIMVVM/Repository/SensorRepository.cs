using KomModule;
using MotorXPGUIMVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorXPGUIMVVM.Repository
{
    public class SensorRepository : IRepository
    {
        private ICommunicator _com;
        private ulong _timeStamp;

        public ulong GetElectroMagneticTorque()
        {
            if(CheckTimeStampAndInit())
            {
                return _com.getData().DataTable.FirstOrDefault(v => 
                                                                (v.Key >= Properties.Settings.Default.ElectroMagneticTorqueKeyMin &&
                                                                 v.Key < Properties.Settings.Default.ElectroMagneticTorqueKeyMax)).Value;
            }
            return 0;
        }

        private bool CheckTimeStampAndInit()
        {
            var newStamp = _com.getData().Timestamp;
            if(_com.isInitialized() && newStamp > _timeStamp)
            {
                _timeStamp = newStamp;
                return true;
            }
            return false;

        }

        public bool GetHallEffect()
        {
            throw new NotImplementedException();
        }

        public ulong GetRotorSpeed()
        {
            throw new NotImplementedException();
        }

        public ulong GetStatorCurrent()
        {
            throw new NotImplementedException();
        }

        public DateTime GetTimeStamp()
        {
            throw new NotImplementedException();
        }

        public Action NewValueArrived()
        {
            throw new NotImplementedException();
        }
    }
}
