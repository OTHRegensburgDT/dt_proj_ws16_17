using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomModule
{
    class RegulationParams
    {
        private UInt64 timestamp;
        private KeyValuePair<UInt16, UInt64> paramTable;

        public KeyValuePair<UInt16, UInt64> ParamTable
        {
            get { return paramTable; }
            set { paramTable = value; }
        }

        public UInt64 Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
    }
}
