using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MotorXPGUIMVVM.Model;
using MotorXPGUIMVVM.Repository;

namespace MotorXPGUIMVVM.ViewModel
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DataDisplayViewModel : ViewModelBase
    {
        private ISensorRepository _repository;

        public DataDisplayViewModel(ISensorRepository repository)
        {
            _repository = repository;
            repository.SensorDataCollections.ListChanged += SensorDataCollectionsOnListChanged;
        }

        private void SensorDataCollectionsOnListChanged(object sender, ListChangedEventArgs listChangedEventArgs)
        {
            var asList = sender as BindingList<SensorDataCollection>;
            if (asList == null) return;
            foreach (var sensorDataCollection in asList)
            {
                if (sensorDataCollection.ShowAllCommand == null) sensorDataCollection.ShowAllCommand = new RelayCommand<object>(OnSensorDataCollectionShowAll);
            }
        }

        private void OnSensorDataCollectionShowAll(object o)
        {
            var asCollection = o as SensorDataCollection;
            if (asCollection != null) asCollection.ShowAll = !asCollection.ShowAll;
        }

        public ISensorRepository Repository
        {
            get { return _repository; }
            set
            {
                // ReSharper disable once ExplicitCallerInfoArgument
                Set(ref _repository, value, nameof(Repository));
            }
        }
    }
}
