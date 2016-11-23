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
        public ulong GetStatorCurrent()
        {
            return (ulong)rand.Next(0, 24);
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
        public ulong GetRotorSpeed()
        {
            return (ulong)rand.Next(1, 3500);
        }

        /// <summary>
        /// Drehmoment in Nm
        /// </summary>
        public ulong GetElectroMagneticTorque()
        {
            return  (ulong)rand.Next(0, 5);
        }

        public DateTime GetTimeStamp()
        {
            return DateTime.Now;
        }

        public Action NewValueArrived()
        {
            throw new NotImplementedException();
        }

        ulong IRepository.GetTimeStamp()
        {
            throw new NotImplementedException();
        }
    }

     public class DemoData
    {
        private SortedList<ushort, ulong> values;

        public DemoData()
        {
            values = new SortedList<ushort, ulong>();
        }
    }
}