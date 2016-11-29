using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MotorXPGUIMVVM.Views
{
    /// <summary>
    /// Interaction logic for GaugeAndLineChartTest.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class GaugeAndLineChartTest : UserControl
    {
        private int _counter = 20;
        private Random rand;
        public GaugeAndLineChartTest()
        {
            rand = new Random();
            InitializeComponent();

            // only run this test code when not in the designer.
            if (DesignerProperties.GetIsInDesignMode(this) == false)
            {
                Task.Run(() =>
                {
                    {
                        while (true)
                        {
                            while (_counter++ < 500)
                            {
                                var j = _counter;
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    Gauge2.Value = Convert.ToDouble(rand.Next(j - 20, j + 20)) / 500 * 100;
                                    //LineChart.AddValue(Gauge.Value);
                                    LineChart.AddValue(Gauge2.Value);
                                });
                                Thread.Sleep(50);
                            }
                            while (_counter-- > 20)
                            {
                                var j = _counter;
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    Gauge2.Value = Convert.ToDouble(rand.Next(j - 20, j + 20)) / 500 * 100;
                                    LineChart.AddValue(Gauge2.Value);
                                    //LineChart2.AddValue(Gauge2.Value);
                                });
                                Thread.Sleep(50);
                            }
                        }
                    }
                    // ReSharper disable once FunctionNeverReturns
                });
            }
        }
    }
}
