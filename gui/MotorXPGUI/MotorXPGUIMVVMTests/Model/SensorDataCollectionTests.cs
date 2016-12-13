using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotorXPGUIMVVM.Model;

namespace MotorXPGUIMVVMTests.Model
{
    [TestClass]
    public class SensorDataCollectionTests
    {
        [TestMethod]
        public void SensorDataCollectionInitVelocityTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity);
            Assert.IsTrue(collection.SensorDataType == SensorDataType.Velocity);
        }
        [TestMethod]
        public void SensorDataCollectionInitAngleTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Angle);
            Assert.IsTrue(collection.SensorDataType == SensorDataType.Angle);
        }
        [TestMethod]
        public void SensorDataCollectionInitTempTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Temp);
            Assert.IsTrue(collection.SensorDataType == SensorDataType.Temp);
        }
        [TestMethod]
        public void SensorDataCollectionInitHallTest()
        {
            var collection = new SensorDataCollection(SensorDataType.HallPattern);
            Assert.IsTrue(collection.SensorDataType == SensorDataType.HallPattern);
        }

        [TestMethod]
        public void SensorDataCollectionSetLastStampTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity) { LastTimeStamp = ulong.MaxValue };
            Assert.IsTrue(collection.LastTimeStamp == ulong.MaxValue);
        }
        [TestMethod]
        public void SensorDataCollectionSetSampleWindowMaxTest()
        {

            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                LastTimeStamp = ulong.MaxValue,
                SampleWindow = ulong.MaxValue
            };
            Assert.IsTrue(collection.SampleWindow == ulong.MaxValue);
        }
        [TestMethod]
        public void SensorDataCollectionSetSampleWindowMinTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity) { SampleWindow = ulong.MinValue };
            Assert.IsTrue(collection.SampleWindow == 10);
        }

        [TestMethod]
        public void SensorDataCollectionSetSampleWindowLowTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                LastTimeStamp = 10,
                SampleWindow = 10
            };
            Assert.IsTrue(collection.SampleWindow == 10);
        }

        [TestMethod]
        public void SensorDataCollectionGetUnitTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity);
            Assert.IsTrue(collection.Unit == "Velocity");
        }

    }

}