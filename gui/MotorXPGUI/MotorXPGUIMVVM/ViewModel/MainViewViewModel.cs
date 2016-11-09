using GalaSoft.MvvmLight;
using MotorXPGUIMVVM.Model;
using System.Diagnostics;

namespace MotorXPGUIMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewViewModel(IDataService dataService)
        {
            _dataService = dataService;
            
        }

        public int GaugeValue
        {
            get {return 2344; }            
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}