using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        bool getDataFlag;
        [TestMethod]
        public void TestMethod1()
        {
            getDataFlag = false;
            KomModule.Sensordata data;
            SortedList<ushort, ulong> list = new SortedList<ushort,ulong>();
            list.Add(73, 65555);
            KomModule.ICommunicator com = new KomModule.UartCommunicator("Com1");
            com.newSensordata += cbGetData;

            while (getDataFlag == false) { };

            data = com.getData();

            Assert.AreEqual(data.DataTable, list);
        }

        public void cbGetData()
        {
            getDataFlag = true;
        }
    }
}
