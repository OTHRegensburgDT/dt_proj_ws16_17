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

        public double GetElectroMagneticTorque()
        {
            throw new NotImplementedException();
        }

        public bool GetHallEffect()
        {
            throw new NotImplementedException();
        }

        public double GetRotorSpeed()
        {
            throw new NotImplementedException();
        }

        public double GetStatorCurrent()
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
