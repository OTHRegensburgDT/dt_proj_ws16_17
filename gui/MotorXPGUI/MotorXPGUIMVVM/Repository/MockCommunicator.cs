using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using KomModule;

namespace MotorXPGUIMVVM.Repository
{
    /// <summary>
    /// MockCommunicator can be used to run the gui without a connected Motor
    /// </summary>
    public class MockCommunicator : ICommunicator
    {
        private static int _seqNumber;
        private readonly Random _rnd;
        private double _angle;
        private double _currentVelocity;
        private double _currentTemperature;
        private RegulationParams _params;

        public MockCommunicator()
        {
            _rnd = new Random();
            StartSensorTask();
            InitSetParams();
        }

        /// <summary>
        /// Initialize the params field
        /// </summary>
        private void InitSetParams()
        {
            var newRegPara = new RegulationParams
            {
                ParamP = 0.6f,
                ParamI = 1.6f,
                ParamD = 0.2f,
                RegTarget = ReguTarget.Velocity,
                TargetVal = 1500
            };
            SetParams(newRegPara);
        }

        /// <summary>
        /// get a new hall pattern calculated from the angle  result
        /// </summary>
        /// <returns> all three values as bitmask in one double</returns>
        private double AddNewHallValue()
        {
            var newHallValue = (int)GetHallA(_angle);
            newHallValue += (int)GetHallB(_angle) << 1;
            newHallValue += (int)GetHallC(_angle) << 2;

            return newHallValue;
        }

        /// <summary>
        /// get a fake temperature for different velocity values
        /// </summary>
        /// <returns>temperature as double</returns>
        private double AddNewTempValue()
        {
            var target = 65;
            if (_params.TargetVal <= 1500)
            {
                target = 35;
                return _currentTemperature += (target - _currentTemperature) * 0.01;
            }
            if (_params.TargetVal >= 4500)
            {
                target = 150;
                return _currentTemperature += (target - _currentTemperature) * 0.01;
            }
            return _currentTemperature += (target - _currentTemperature) * 0.01;           
        }

        /// <summary>
        /// get a new velocity value to the sensordata object for getdata
        /// </summary>
        /// <returns>the new Angle as double</returns>
        private double AddNewAngleValue()
        {
            const int maxAngle = 120;
            const int maxVelocity = 6000;
            Angle += (_currentVelocity/maxVelocity)*maxAngle;

            return Angle;
            
        }
       
        /// <summary>
        /// add a new velocity value to the sensordata object for getdata
        /// 
        /// </summary>
        /// <returns>the return value is near the targetvalue as double</returns>
        private double AddNewVelocityValue()
        {
            var target = _params.TargetVal;

            var retValue = _currentVelocity += (target - _currentVelocity) * 0.075;
            return retValue < 0 ? 0 : retValue;
        }

        /// <summary>         
        ///  start a task to trigger the newSensordata tast all 100 to 1000 ms
        /// </summary>
        private void StartSensorTask()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (Application.Current == null) continue;
                    Application.Current.Dispatcher.Invoke(() => { NewSensordata?.Invoke(); });
                    Thread.Sleep(50);
                }
                // ReSharper disable once FunctionNeverReturns
            });
        }

        /// <summary>
        /// private property for the angle, value between 0 and 360
        /// </summary>
        private double Angle
        {
            get { return _angle; }
            set { _angle = value%359; }
        }

        /// <summary>
        /// get a hall value for A
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private static double GetHallA(double angle)
        {
            var preAngle = PrepareAngle(angle);
            if ((preAngle >= -60.0) && (preAngle <= 120)) return 1.0;
            return 0;
        }
        // get a hall value for B
        private static double GetHallB(double angle)
        {
            var preAngle = PrepareAngle(angle);
            if ((preAngle >= 60.0) || (preAngle <= -120)) return 1.0;
            return 0;
        }
        // get a hall value for C
        private static double GetHallC(double angle)
        {
            var preAngle = PrepareAngle(angle);
            if ((preAngle >= -180.0) && (preAngle <= 0)) return 1.0;
            return 0;
        }

        /// <summary>
        ///  preparing the angle to get a valid hall pattern
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private static double PrepareAngle(double angle)
        {
            var sin = Math.Sin(angle);
            var cos = Math.Cos(angle);
            var aTan2 = Math.Atan2(sin, cos);
            return aTan2*(180/Math.PI);
        }

        /// <summary>
        /// Get a new sensordata object with new values
        /// </summary>
        /// <returns></returns>
        public Sensordata GetData()
        {
            var data = new Sensordata
            {
                SeqNr = (ulong) _seqNumber++,
                DataTable = new SortedList<ushort, double>
                {
                    {1, AddNewVelocityValue()},
                    {2, AddNewAngleValue()},
                    {3, AddNewTempValue()},
                    {4, AddNewHallValue()}
                }
            };

            return data;
        }

        /// <summary>
        /// event trigger if new datas are arrived
        /// </summary>
        public event Action NewSensordata;

        /// <summary>
        /// return allways true that the new data are recived
        /// </summary>
        /// <returns>allways true</returns>
        public bool SendParams()
        {
            Thread.Sleep(_rnd.Next(100,1000));
            return true;
        }

        /// <summary>
        /// set the recived params
        /// </summary>
        /// <param name="para">new RegulationParams</param>
        /// <returns>allways true</returns>
        public bool SetParams(RegulationParams para)
        {
            _params = para;
            return true;
        }
        
        /// <summary>
        /// indicates that the contorller is initialized
        /// </summary>
        /// <returns>allways true</returns>
        public bool IsInitialized()
        {
            return true;
        }
    }
}