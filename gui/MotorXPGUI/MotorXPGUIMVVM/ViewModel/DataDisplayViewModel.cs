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
        private float _valueP;
        private float _valueI;
        private float _valueD;

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
                if (sensorDataCollection.ShowAllCommand == null)
                {
                    sensorDataCollection.ShowAllCommand = new RelayCommand<object>(OnSensorDataCollectionShowAll);                    
                }
            }
        }

        public float ValueP
        {
            get { return _valueP; }
            set
            {
                _valueP = value;
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(ValueP));
            }
        }

        public float ValueI
        {
            get { return _valueI; }
            set
            {
                _valueI = value;
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(ValueI));
            }
        }

        public float ValueD
        {
            get { return _valueD; }
            set
            {
                _valueD = value;
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(ValueD));
            }
        }

        private void OnSensorDataCollectionShowAll(object o)
        {
            var asCollection = o as SensorDataCollection;
            if (asCollection != null) asCollection.ShowAll = !asCollection.ShowAll;
        }

        private void OnSubmitPIDCommand(object o)
        {
            if (_repository.SubmitPIDCommand == null)
            {
                _repository.SubmitPIDCommand = new RelayCommand<object>(OnSubmitPIDCommand);
            }
            _repository.SendPID();
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
