using System;
using System.Collections.Generic;

namespace MotorXPGUIMVVM.Model
{
    public class SensorData
    {
        public UInt64 Timestamp { get; set; }
        public SortedList<UInt16, UInt64> DataTable { get; set; }
    }
}
