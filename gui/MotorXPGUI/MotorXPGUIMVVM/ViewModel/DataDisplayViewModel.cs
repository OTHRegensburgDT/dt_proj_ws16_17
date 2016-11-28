using GalaSoft.MvvmLight;
using MotorXPGUIMVVM.Repository;

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
