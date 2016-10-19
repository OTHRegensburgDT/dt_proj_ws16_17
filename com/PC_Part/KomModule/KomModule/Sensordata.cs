using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomModule
{
    public class Sensordata
    {
        private UInt64 timestamp;
        private SortedList<UInt16, UInt64> dataTable;

        public UInt64 Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public SortedList<UInt16, UInt64> DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }
    }
}
