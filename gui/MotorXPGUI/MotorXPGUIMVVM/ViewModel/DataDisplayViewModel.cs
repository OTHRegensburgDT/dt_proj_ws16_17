using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KomModule;
using MotorXPGUIMVVM.Model;
using MotorXPGUIMVVM.Repository;
using System.ComponentModel;

namespace MotorXPGUIMVVM.ViewModel
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DataDisplayViewModel : ViewModelBase
    {
        private ISensorRepository _repository;
        private float _valueP = 0.6f;
        private float _valueI = 0.2f;
        private float _valueD = 0.03f;
        private float _valueTarget = 100;
        private ReguTarget _reguTarget = ReguTarget.Velocity;

        public DataDisplayViewModel(ISensorRepository repository)
        {
            _repository = repository;
            repository.SubmitPIDCommand = new RelayCommand<object>(OnSubmitPIDCommand);
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

        public float ValueTarget
        {
            get { return _valueTarget; }
            set
            {
                _valueTarget = value; 
                RaisePropertyChanged();
            }
        }

        public ReguTarget ReguTarget
        {
            get { return _reguTarget; }
            set
            {
                _reguTarget = value; 
                RaisePropertyChanged();
            }
        }

        private static void OnSensorDataCollectionShowAll(object o)
        {
            var asCollection = o as SensorDataCollection;
            if (asCollection != null) asCollection.ShowAll = !asCollection.ShowAll;
        }

       
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private void OnSubmitPIDCommand(object o)
        {
            var regParams = new RegulationParams
                // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            {
                ParamP = ValueP,
                ParamD = ValueD,
                ParamI = ValueI,
                TargetVal = ValueTarget,
                RegTarget = ReguTarget.Velocity
            };
            _repository.SendPID(regParams);
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
