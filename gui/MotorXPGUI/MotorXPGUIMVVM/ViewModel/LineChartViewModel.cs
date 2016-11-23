using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MotorXPGUIMVVM.ViewModel
{
    public class LineChartViewModel : ViewModelBase
    {
        private string _unit;
        private string _title;

        private ObservableCollection<KeyValuePair<ulong, ulong>> _items;

        public LineChartViewModel()
        {
           
        }

        private ObservableCollection<KeyValuePair<ulong, ulong>> GetSample()
        {
            var items = new ObservableCollection<KeyValuePair<ulong, ulong>>();
            items.Add(new KeyValuePair<ulong, ulong>(key: 4122, value: 12));
            items.Add(new KeyValuePair<ulong, ulong>(key: 4123, value: 15));
            items.Add(new KeyValuePair<ulong, ulong>(key: 4124, value: 19));
            items.Add(new KeyValuePair<ulong, ulong>(key: 4125, value: 25));
            return items;
        }


        public string Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                RaisePropertyChanged("Unit");
            }
        }


        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
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
                RaisePropertyChanged("Items");
            }
        }
    }
}
