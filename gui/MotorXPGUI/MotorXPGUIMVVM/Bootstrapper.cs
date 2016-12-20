using AutoMapper;
using KomModule;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using MotorXPGUIMVVM.Model;
using MotorXPGUIMVVM.Repository;
using MotorXPGUIMVVM.ViewModel;
using System;
using System.Collections.Generic;

namespace MotorXPGUIMVVM
{
    public class Bootstrapper : IServiceLocator, IDisposable
    {
        private IUnityContainer Container { get; }

        public Bootstrapper()
        {
            Container = new UnityContainer();
            ConfigurationContainer();
        }

        private void ConfigurationContainer()
        {
            var config = new MapperConfiguration(cfg =>
            { 
                cfg.CreateMap<SensorData, Sensordata>();
            });
            var mapper = config.CreateMapper();
            Container.RegisterType<MainViewModel>()
                .RegisterInstance(mapper)
                .RegisterType<MainViewViewModel>()
                .RegisterType<DataDisplayViewModel>()
                //.RegisterType<ISensorRepository, SensorRepository>();
                .RegisterType<ISensorRepository, MockSensorRepository>();
 
        }
        

        public object GetInstance(Type serviceType)
        {
            return Container.Resolve(serviceType);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return Container.Resolve(serviceType, key);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return Container.ResolveAll(serviceType);
        }

        public TService GetInstance<TService>()
        {
            return Container.Resolve<TService>();
        }

        public TService GetInstance<TService>(string key)
        {
            return Container.Resolve<TService>(key);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return Container.ResolveAll<TService>();
        }

        public object GetService(Type serviceType)
        {
            return Container.Resolve(serviceType);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}
