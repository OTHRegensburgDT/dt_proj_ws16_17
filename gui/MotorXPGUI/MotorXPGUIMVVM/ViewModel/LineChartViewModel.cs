using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MotorXPGUIMVVM.ViewModel
{
    public class LineChartViewModel : ViewModelBase
    {
        private string _unit;
        private string _title;

        private ObservableCollection<KeyValuePair<ulong, ulong>> _items;
        // ReSharper disable once UnusedMember.Local
        private ObservableCollection<KeyValuePair<ulong, ulong>> GetSample()
        {
            var items = new ObservableCollection<KeyValuePair<ulong, ulong>>
            {
                new KeyValuePair<ulong, ulong>(key: 4122, value: 12),
                new KeyValuePair<ulong, ulong>(key: 4123, value: 15),
                new KeyValuePair<ulong, ulong>(key: 4124, value: 19),
                new KeyValuePair<ulong, ulong>(key: 4125, value: 25)
            };
            return items;
        }


        public string Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(Unit));
            }
        }


        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(Title));
            }
        }



        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public ObservableCollection<KeyValuePair<ulong, ulong>> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(Items));
            }
        }
    }
}
