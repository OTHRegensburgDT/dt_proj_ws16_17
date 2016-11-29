using System;
using System.Threading;
using System.Threading.Tasks;
using MotorXPGUIMVVM.Properties;

namespace MotorXPGUIMVVM.Repository
{
    public class Metronom
    {
        private Action _tick;

        public event Action Tick
        {
            add { _tick += value; }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_tick != null) _tick -= value;
            }
        }
        private bool _shouldStop;
        private int _beatsPerMinute = 120;

        public void Start()
        {
            _shouldStop = false;
            var task = new Task(ErzeugeTicks);
            task.Start();
        }

        private void ErzeugeTicks()
        {
            var intervallInMilliseconds = (60.0 / _beatsPerMinute) * 1000.0;
            var interval = TimeSpan.FromMilliseconds(intervallInMilliseconds);

            while(_shouldStop == false)
            {
                if (_tick != null)
                {
                    _tick();
                }
                Thread.Sleep(interval);
            }
        }

        public void Stop()
        {
            _shouldStop = true;
        }

        /// <summary>
        /// get and set beats per minutes
        /// 
        /// throws ArgumentOutOfRangeException if BPM is smaller than 1
        /// </summary>
        public int BeatsPerMinute
        {
            get { return _beatsPerMinute; }
            set
            {
                if(value <1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), Resources.Metronom_BeatsPerMinute_BPM_must_be_grather_than_0_seconds);
                }
                _beatsPerMinute = value;
            }
    }
}

}