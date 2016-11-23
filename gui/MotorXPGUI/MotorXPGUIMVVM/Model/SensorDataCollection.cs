using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MotorXPGUIMVVM.Annotations;

namespace MotorXPGUIMVVM.Model
{
    public class SensorDataCollection :  INotifyPropertyChanged
    {
        private BindingList<double> _values;

        public SensorDataCollection(SensorDataType type)
        {
            SensorDataType = type;
            Values = new BindingList<double>();
        }

        public SensorDataType SensorDataType { get; } 

        public BindingList<double> Values
        {
            get { return _values; }
            private set
            {
                _values = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
