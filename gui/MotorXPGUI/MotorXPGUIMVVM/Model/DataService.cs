using AutoMapper;
using KomModule;
using System;

namespace MotorXPGUIMVVM.Model
{
    public class DataService : IDataService
    {
        private SensorData _data;
        private ICommunicator _com;
       
        public SensorData Test()
        {
            throw new NotImplementedException();
        }
    }
}