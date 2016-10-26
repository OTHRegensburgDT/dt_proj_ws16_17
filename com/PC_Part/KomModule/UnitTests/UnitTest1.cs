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

        [TestMethod]
        public void TestFrameParsing()
        {
            byte[] buf = {1, 2, 3, 4, 5, 6, 7, 8, 9};
            byte[] output, frame;
            frame = KomModule.Frameparser.EncapsuleFrame(buf);
            output = KomModule.Frameparser.DecapsuleFrame(frame);
            Assert.AreEqual(buf,output);
        }
    }
}
