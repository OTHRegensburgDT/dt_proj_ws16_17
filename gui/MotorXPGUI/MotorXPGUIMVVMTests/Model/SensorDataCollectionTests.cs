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
        [TestMethod]
        public void SensorDataCollectionGetCurrentValueWithoutShowAllTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                ShowAll = false,
                CurrentValue = 1500
            };
            Assert.IsTrue(Math.Abs(collection.CurrentValue - 1500) < 1);
        }
        [TestMethod]
        public void SensorDataCollectionGetCurrentValueWithShowAllTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                ShowAll = true,
                CurrentValue = 1500,
                LastValue = 500
            };
            Assert.IsTrue(Math.Abs(collection.CurrentValue - 500) < 1);
        }
        [TestMethod]
        public void SensorDataCollectionGetCurrentValueTextTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                ShowAll = false,
                CurrentValue = 1500
            };
            Assert.IsTrue(collection.CurrentValueText == "Current Value: 1500");
        }
        [TestMethod]
        public void SensorDataCollectionSetTargetValueToHighTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                TargetValue = 6001
            };
            Assert.IsTrue(collection.TargetValue == collection.MaxValue);
        }
        [TestMethod]
        public void SensorDataCollectionSetTargetValueHighTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                TargetValue = 6000
            };
            Assert.IsTrue(collection.TargetValue == 6000);
        }
        [TestMethod]
        public void SensorDataCollectionSetTargetValueToLowTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                TargetValue = -1
            };
            Assert.IsTrue(collection.TargetValue == collection.MinValue);
        }
        [TestMethod]
        public void SensorDataCollectionSetTargetValueLowTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                TargetValue = 0
            };
            Assert.IsTrue(collection.TargetValue == 0);
        }
        [TestMethod]
        public void SensorDataCollectionGetInitialShowAllTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity);
            Assert.IsTrue(collection.ShowAll);
        }
        [TestMethod]
        public void SensorDataCollectionSetShowAllTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                ShowAll = false
            };
            Assert.IsFalse(collection.ShowAll);
        }
        [TestMethod]
        public void SensorDataCollectionSetShowAllAndCheckSampleWindowTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                ShowAll = true,
                //LastTimeStamp have to be greater 10 this is the MinValue for the SampleWindow
                LastTimeStamp =  11
            };
            Assert.IsTrue(collection.SampleWindow == collection.LastTimeStamp);
        }
        [TestMethod]
        public void SensorDataCollectionSetShowAllAndCheckSampleWindowTest2()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                ShowAll = true,
                LastTimeStamp = 9
            };
            //minValue of SampleWindow is 10
            Assert.IsTrue(collection.SampleWindow == 10);
        }
        [TestMethod]
        public void SensorDataCollectionCheckSampleWindowToHighTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                //LastTimeStamp have to be greater 10 this is the MinValue for the SampleWindow
                LastTimeStamp = 11,
                SampleWindow = 20
            };
            Assert.IsTrue(collection.SampleWindow == collection.LastTimeStamp);
        }
        [TestMethod]
        public void SensorDataCollectionCheckSampleWindowToLowTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                //LastTimeStamp have to be greater 10 this is the MinValue for the SampleWindow
                LastTimeStamp = 11,
                SampleWindow = 0
            };
            Assert.IsTrue(collection.SampleWindow == collection.LastTimeStamp);
        }
        [TestMethod]
        public void SensorDataCollectionCheckSampleWindowToLowWithoutShowAllTest()
        {
            var collection = new SensorDataCollection(SensorDataType.Velocity)
            {
                //LastTimeStamp have to be greater 10 this is the MinValue for the SampleWindow
                ShowAll = false,
                LastTimeStamp = 11,
                SampleWindow = 0
            };
            Assert.IsTrue(collection.SampleWindow == 10);
        }
    }
}