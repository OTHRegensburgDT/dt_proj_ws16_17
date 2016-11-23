using GalaSoft.MvvmLight;
using MotorXPGUIMVVM.Controls.Gauge;
using MotorXPGUIMVVM.Controls.LineChart;
using MotorXPGUIMVVM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorXPGUIMVVM.ViewModel
{
    public class DataDisplayViewModel : ViewModelBase
    {
        private ISensorRepository _repository;

        public DataDisplayViewModel(ISensorRepository repository)
        {
            _repository = repository;
        }

        public ISensorRepository Repository
        {
            get { return _repository; }
            set
            {
                Set(ref _repository, value, nameof(Repository));
            }
        }
    }
}
