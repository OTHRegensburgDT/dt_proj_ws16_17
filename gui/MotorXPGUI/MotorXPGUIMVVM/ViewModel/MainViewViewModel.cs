using GalaSoft.MvvmLight;
using MotorXPGUIMVVM.Model;
using MotorXPGUIMVVM.Repository;
using System.Diagnostics;
using System;

namespace MotorXPGUIMVVM.ViewModel
{
    public class MainViewViewModel : ViewModelBase
    {

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewViewModel() 
        {
            //_repo = repo;

            InitSpeedViewModels();
            InitTempViewModels();
        }

        private void InitTempViewModels()
        {
            TempViewModel = new GaugeViewModel();
            TempViewModel.Title = Properties.Settings.Default.TempTitle;
            TempViewModel.Unit = Properties.Settings.Default.TempUnit;
            TempViewModel.MinValue = Properties.Settings.Default.TempMinValue;
            TempViewModel.MaxValue = Properties.Settings.Default.TempMaxValue;
            TempViewModel.GaugeValue = 42.0;

            TempLineVM = new LineChartViewModel();
            TempLineVM.MinValue = Properties.Settings.Default.TempMinValue;
            TempLineVM.MaxValue = Properties.Settings.Default.TempMaxValue;
            TempLineVM.Title = Properties.Settings.Default.TempTitle;
            TempLineVM.Unit = Properties.Settings.Default.TempUnit;

        }

        private void InitSpeedViewModels()
        {
            SpeedViewModel =  new GaugeViewModel();
            SpeedViewModel.Title = Properties.Settings.Default.SpeedTitle;
            SpeedViewModel.Unit = Properties.Settings.Default.SpeedUnit;
            SpeedViewModel.MinValue = Properties.Settings.Default.SpeedMinValue;
            SpeedViewModel.MaxValue = Properties.Settings.Default.SpeedMaxValue;
            SpeedViewModel.GaugeValue = 42.0;

            SpeedLineVM = new LineChartViewModel();
            SpeedLineVM.MinValue =  Properties.Settings.Default.SpeedMinValue;
            SpeedLineVM.MaxValue =  Properties.Settings.Default.SpeedMaxValue;
            SpeedLineVM.Title =     Properties.Settings.Default.SpeedTitle;
            SpeedLineVM.Unit =      Properties.Settings.Default.SpeedUnit;
        }

        public GaugeViewModel SpeedViewModel { get; set; }
        public GaugeViewModel TempViewModel { get; set; } 
        public LineChartViewModel SpeedLineVM { get; set; }
        public LineChartViewModel TempLineVM { get; set; }
        
        
        
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}