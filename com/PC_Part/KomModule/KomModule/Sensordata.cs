using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomModule
{
    public class Sensordata
    {
        public ulong SeqNr { get; set; }

        public SortedList<ushort, double> DataTable { get; set; }
    }
}
