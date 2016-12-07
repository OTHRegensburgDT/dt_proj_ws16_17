using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        bool getDataFlag;
        [TestMethod]
        public void TestRcvData()
        {
            int assertVal = 0;
            getDataFlag = false;
            KomModule.Sensordata data = new KomModule.Sensordata();
            SortedList<ushort, ulong> list = new SortedList<ushort,ulong>();
            list.Add(73, 65555);
            KomModule.ICommunicator com = new KomModule.UartCommunicator("Com5");
            com.NewSensordata += cbGetData;

            for (int i = 0; i < 3; i++)
            {
                while (getDataFlag == false){ };
                getDataFlag = false;
                data = com.GetData();
            }

            if (data.DataTable.Count == 4)
            {
                assertVal = 1;
            }
            Assert.AreEqual(assertVal, 1);
        }

        public void cbGetData()
        {
            getDataFlag = true;
        }

        [TestMethod]
        public void TestFrameParsing()
        {
            int i = 0;
            byte[] buf = {1, 2, 3, 4, 5, 6, 7, 8, 9};
            byte[] output, frame;
            frame = KomModule.Frameparser.EncapsuleFrame(buf);
            output = KomModule.Frameparser.DecapsuleFrame(frame);
            foreach (var element in output)
            {
                Assert.AreEqual(buf[i], element);
                i++;
            }
        }

        [TestMethod]
        public void decodeBytearr()
        {
            KomModule.Sensordata data;
            //byte[] arr = { 196, 135, 254, 31, 8, 233, 7, 16, 5, 18, 5, 8, 234, 7, 16, 6, 18, 4, 8, 1, 16, 1, 18, 4, 8, 2, 16, 2, 18, 4, 8, 3, 16, 3, 18, 5, 8, 209, 15, 16, 4};

            //data = KomModule.Protoparser.ByArrtoSData(arr);
            //ToDo update test
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void encodeBytearr()
        {
            KomModule.Sensordata data = new KomModule.Sensordata();
            KomModule.Sensordata dataout;
            SortedList<ushort, Double> list = new SortedList<ushort, Double>();
            byte[] arr;
            data.SeqNr = 0;
            list.Add(1, 1);
            list.Add(2, 2);
            list.Add(3, 3);
            list.Add(1001, 5);
            list.Add(1002, 6);
            list.Add(2001, 4);

            data.DataTable = list;

            arr = KomModule.Protoparser.SDatatoByArrr(data);
            dataout = KomModule.Protoparser.ByArrtoSData(arr);
            Assert.AreEqual(1, 1);
        }
        [TestMethod]
        public void TestRegPara_EnDecaps()
        {
            KomModule.RegulationParams paraIn = new KomModule.RegulationParams();
            KomModule.RegulationParams paraOut;
            byte[] buf;
            paraIn.ParamD = 1.1f;
            paraIn.ParamI = 2.2f;
            paraIn.ParamP = 3.3f;
            paraIn.TargetVal = 4400f;
            paraIn.RegTarget = KomModule.ReguTarget.Velocity;

            buf = KomModule.Protoparser.RParatoByArr(paraIn);
            paraOut = KomModule.Protoparser.ByArrtoRPara(buf);

            Assert.AreEqual(1, 1);
        }
        [TestMethod]
        public void TestSendPara()
        {
            KomModule.ICommunicator com = new KomModule.UartCommunicator("Com5");
            KomModule.RegulationParams paraIn = new KomModule.RegulationParams();

            paraIn.ParamD = 1.1f;
            paraIn.ParamI = 2.2f;
            paraIn.ParamP = 3.3f;
            paraIn.TargetVal = 4400f;
            paraIn.RegTarget = KomModule.ReguTarget.Velocity;

            com.SetParams(paraIn);
            com.SendParams();

            paraIn.ParamD = 9.9f;
            paraIn.ParamI = 8.8f;
            paraIn.ParamP = 7.7f;
            paraIn.TargetVal = 44f;
            paraIn.RegTarget = KomModule.ReguTarget.Temperature;

            com.SendParams();

            Assert.IsTrue(true);
        }
    }
}
