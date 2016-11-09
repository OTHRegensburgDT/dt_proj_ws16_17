using System;
using System.Linq;
using System.Collections.Generic;
using MotorXPGUIMVVM.Model;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MotorXPGUIMVVM.Repository
{
    public class DemoRepository : IRepository
    {
        private Random rand = new Random();

        /// <summary>
        /// Strom der durch Spule fließt
        /// </summary>
        public double GetStatorCurrent()
        {
            return rand.Next(0, 24);
        }

        /// <summary>
        /// Hall sensor Magnetfeld wechsel
        /// </summary>
        public bool GetHallEffect()
        {
            return (rand.Next(0, 1) > 0) ? true : false;
        }

        /// <summary>
        /// Drehgeschwindigkeit in Rad/Sec
        /// </summary>
        public double GetRotorSpeed()
        {
            return rand.Next(1, 3500);
        }

        /// <summary>
        /// Drehmoment in Nm
        /// </summary>
        public double GetElectroMagneticTorque()
        {
            return rand.Next(0, 5);
        }

        public DateTime GetTimeStamp()
        {
            return DateTime.Now;
        }

        public Action NewValueArrived()
        {
            throw new NotImplementedException();
        }
    }
}