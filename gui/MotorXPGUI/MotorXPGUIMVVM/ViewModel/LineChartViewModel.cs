using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorXPGUIMVVM.ViewModel
{
    public class LineChartViewModel
    {
        public LineChartViewModel()
        {

        }

        public string Unit { get; set; }
        public string Title { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public List<double> Values { get; set; }

    }
}
