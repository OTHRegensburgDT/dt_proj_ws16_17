using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomModule
{
    public class Sensordata
    {
        private UInt64 seqNr;
        private SortedList<UInt16, Double> dataTable;

        public UInt64 SeqNr
        {
            get { return seqNr; }
            set { seqNr = value; }
        }

        public SortedList<UInt16, Double> DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }
    }
}
