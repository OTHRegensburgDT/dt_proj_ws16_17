using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomModule
{
    class Parser
    {
        internal static Sensordata StrtoSData(string InputData)
        {
            Sensordata retVal = new Sensordata();
            SortedList<ushort, ulong> dataTab = new SortedList<ushort,ulong>();
            dataTab.Add(73, 65555);
            retVal.DataTable = dataTab;

            return retVal;
        }
    }
}
