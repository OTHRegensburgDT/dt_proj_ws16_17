using System;
using GalaSoft.MvvmLight;
using KomModule;
using MotorXPGUIMVVM.Model;
using MotorXPGUIMVVM.Repository;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;

namespace MotorXPGUIMVVM.ViewModel
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DataDisplayViewModel : ViewModelBase
    {
        private ISensorRepository _repository;
        private float _valueP = 0.6f;
        private float _valueI = 0.2f;
        private float _valueD = 0.03f;
        private ReguTarget _reguTarget = ReguTarget.Velocity;
        private bool _isBusy;

        public DataDisplayViewModel(ISensorRepository repository)
        {
            _repository = repository;
            repository.SubmitPIDCommand = new GalaSoft.MvvmLight.Command.RelayCommand<object>(OnSubmitPIDCommand);
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
                    sensorDataCollection.ShowAllCommand = new GalaSoft.MvvmLight.Command.RelayCommand<object>(OnSensorDataCollectionShowAll);                    
                }
                if(sensorDataCollection.SliderMouseButtonUpCommand == null)
                    sensorDataCollection.SliderMouseButtonUpCommand = new RelayCommand<object>(OnSliderMouseButtonUp);
            }
        }

        private void OnSliderMouseButtonUp(object o)
        {
            OnSubmitPIDCommand(null);
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

        public ReguTarget ReguTarget
        {
            get { return _reguTarget; }
            set
            {
                _reguTarget = value; 
                RaisePropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value; 
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsNotBusy));
            }
        }

        public bool IsNotBusy => !IsBusy;


        private static void OnSensorDataCollectionShowAll(object o)
        {
            var asCollection = o as SensorDataCollection;
            if (asCollection != null) asCollection.ShowAll = !asCollection.ShowAll;
        }

       
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private async void OnSubmitPIDCommand(object o)
        {
            IsBusy = true;
            var regParams = new RegulationParams
                // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            {
                ParamP = ValueP,
                ParamD = ValueD,
                ParamI = ValueI,
                RegTarget = ReguTarget.Velocity
            };

            foreach (var collection in _repository.SensorDataCollections)
            {
                switch (collection.SensorDataType)
                {
                    case SensorDataType.Velocity:
                        if (ReguTarget == ReguTarget.Velocity)
                            regParams.TargetVal = collection.TargetValue;
                        break;
                    case SensorDataType.Angle:
                        if (ReguTarget == ReguTarget.Angle)
                            regParams.TargetVal = collection.TargetValue;
                        break;
                    case SensorDataType.Temp:
                        if (ReguTarget == ReguTarget.Temperature)
                            regParams.TargetVal = collection.TargetValue;
                        break;
                }
            }

            await Task.Run(() => _repository.SendPID(regParams));
            IsBusy = false;
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
