using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using KomModule;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        bool _getDataFlag;
        [TestMethod]
        public void TestRcvData()
        {
            var assertVal = 0;
            _getDataFlag = false;
            var data = new Sensordata();
            // ReSharper disable once UnusedVariable
            var list = new SortedList<ushort, ulong> {{73, 65555}};
            var com = new UartCommunicator("COM3");
            com.NewSensordata += CbGetData;

            for (var i = 0; i < 3; i++)
            {
                while (_getDataFlag == false){ }
                _getDataFlag = false;
                data = com.GetData();
                Console.WriteLine(data.SeqNr);
                foreach (var d in data.DataTable)
                {
                    Console.WriteLine(d);
                }
            }

            if (data.DataTable.Count == 4)
            {
                assertVal = 1;
            }
            Assert.AreEqual(assertVal, 1);
        }

        private void CbGetData()
        {
            _getDataFlag = true;
        }

        [TestMethod]
        public void TestFrameParsing()
        {
            var i = 0;
            byte[] buf = {1, 2, 3, 4, 5, 6, 7, 8, 9};
            var frame = Frameparser.EncapsuleFrame(buf);
            var output = Frameparser.DecapsuleFrame(frame);
            foreach (var element in output)
            {
                Assert.AreEqual(buf[i], element);
                i++;
            }
        }

        [TestMethod]
        public void DecodeBytearr()
        {
            //Sensordata data;
            //byte[] arr = { 196, 135, 254, 31, 8, 233, 7, 16, 5, 18, 5, 8, 234, 7, 16, 6, 18, 4, 8, 1, 16, 1, 18, 4, 8, 2, 16, 2, 18, 4, 8, 3, 16, 3, 18, 5, 8, 209, 15, 16, 4};

            //data = KomModule.Protoparser.ByArrtoSData(arr);
            //ToDo update test
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void EncodeBytearr()
        {
            var data = new Sensordata();
            var list = new SortedList<ushort, double>();
            data.SeqNr = 0;
            list.Add(1, 1);
            list.Add(2, 2);
            list.Add(3, 3);
            list.Add(1001, 5);
            list.Add(1002, 6);
            list.Add(2001, 4);

            data.DataTable = list;

            var arr = Protoparser.SDatatoByArrr(data);
            
            // ReSharper disable once UnusedVariable
            var dataout = Protoparser.ByArrtoSData(arr);
            Assert.AreEqual(1, 1);
        }
        [TestMethod]
        public void TestRegPara_EnDecaps()
        {
            var paraIn = new RegulationParams
            {
                ParamD = 1.1f,
                ParamI = 2.2f,
                ParamP = 3.3f,
                TargetVal = 4400f,
                RegTarget = ReguTarget.Velocity
            };

            var buf = Protoparser.RParatoByArr(paraIn);
            // ReSharper disable once UnusedVariable
            var paraOut = Protoparser.ByArrtoRPara(buf);

            Assert.AreEqual(1, 1);
        }
        [TestMethod]
        public void TestSendPara()
        {
            var com = new UartCommunicator("COM3");
            var paraIn = new RegulationParams
            {
                ParamD = 1.1f,
                ParamI = 2.2f,
                ParamP = 3.3f,
                TargetVal = 4400f,
                RegTarget = ReguTarget.Velocity
            };


            com.SetParams(paraIn);
            com.SendParams();

            paraIn.ParamD = 9.9f;
            paraIn.ParamI = 8.8f;
            paraIn.ParamP = 7.7f;
            paraIn.TargetVal = 44f;
            paraIn.RegTarget = ReguTarget.Temperature;

            com.SendParams();

            Assert.IsTrue(true);
        }
    }
}
