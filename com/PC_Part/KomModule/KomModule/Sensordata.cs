using System.Collections.Generic;

namespace KomModule
{
    public class Sensordata
    {
        public ulong SeqNr { get; set; }

        public SortedList<ushort, double> DataTable { get; set; }
    }
}
