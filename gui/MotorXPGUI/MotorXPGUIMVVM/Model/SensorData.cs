using System.Collections.Generic;

namespace MotorXPGUIMVVM.Model
{
    public class SensorData
    {
        public ulong Timestamp { get; set; }
        public SortedList<ushort, ulong> DataTable { get; set; }
    }
}
