using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotorXPGUIMVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorXPGUIMVVM.Model.Tests
{
    [TestClass()]
    public class SensorDataCollectionTests
    {
        [TestMethod()]
        public void SensorDataCollectionInitVelocityTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity);
            Assert.IsTrue(collection.SensorDataType == SensorDataType.Velocity);
        }
        [TestMethod()]
        public void SensorDataCollectionInitAngleTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Angle);
            Assert.IsTrue(collection.SensorDataType == SensorDataType.Angle);
        }
        [TestMethod()]
        public void SensorDataCollectionInitTempTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Temp);
            Assert.IsTrue(collection.SensorDataType == SensorDataType.Temp);
        }
        [TestMethod()]
        public void SensorDataCollectionInitHallTest()
        {
            var collection = new SensorDataCollection(SensorDataType.HallPattern);
            Assert.IsTrue(collection.SensorDataType == SensorDataType.HallPattern);
        }

    }
}