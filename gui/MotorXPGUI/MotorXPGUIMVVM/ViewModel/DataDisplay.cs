using MotorXPGUIMVVM.Controls.Gauge;
using MotorXPGUIMVVM.Controls.LineChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorXPGUIMVVM.ViewModel
{
    public class DataDisplayVM
    {
        private Gauge _gauge;
        private LineChart _line;
        public DataDisplayVM()
        {
            _line = new LineChart();
            _line.AddValue(42);
        }

        

    }
}
