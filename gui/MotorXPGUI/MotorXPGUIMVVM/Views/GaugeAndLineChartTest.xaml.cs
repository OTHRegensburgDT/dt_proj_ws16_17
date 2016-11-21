using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MotorXPGUIMVVM.Views {
    /// <summary>
    /// Interaction logic for GaugeAndLineChartTest.xaml
    /// </summary>
    public partial class GaugeAndLineChartTest : UserControl {
        public GaugeAndLineChartTest() {
            InitializeComponent();
            
            // only run this test code when not in the designer.
            if (DesignerProperties.GetIsInDesignMode(this) == false) 
                Task.Run(() => {
                var i = 500;
                while (i-- > 0 ) {
                    var j = i;
                    Application.Current.Dispatcher.Invoke(() => {
                        Gauge2.Value = Convert.ToDouble(j) / 500 * 100;
                        LineChart.AddValue(Gauge.Value);
                        LineChart2.AddValue(Gauge2.Value);
                    });
                    Thread.Sleep(100);
                }
            });
        }
    }
}
