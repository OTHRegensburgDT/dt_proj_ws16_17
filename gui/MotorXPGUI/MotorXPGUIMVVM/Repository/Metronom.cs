using System;
using System.Threading;
using System.Threading.Tasks;

namespace MotorXPGUIMVVM.Repository
{
    public class Metronom
    {
        private Action _tick;

        public event Action Tick
        {
            add { _tick += value; }
            remove { _tick -= value; }
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
                    throw new ArgumentOutOfRangeException("value", "BPM must be grather than 0 seconds");
                }
                _beatsPerMinute = value;
            }
    }
}

}