using System;
using System.Collections.Generic;
using Google.Protobuf;
using MotorXP.Protobuf.SensorMSg;
using MotorXP.Protobuf.ParamMSg;

namespace KomModule
{
    public static class Protoparser
    {
        public static Sensordata ByArrtoSData(byte[] inputData)
        {
            var retVal = new Sensordata {DataTable = new SortedList<ushort, double>()};


            //create ProtoBuf Message from byteArray
            try
            {
                var protoMsg = SensorMsg.Parser.ParseFrom(inputData);
                //build Sensordata object
                retVal.SeqNr = protoMsg.SequenceNr;
                foreach (var dataEntry in protoMsg.DataTable)
                {
                    retVal.DataTable.Add((ushort)dataEntry.SensorId, dataEntry.Data);
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Input Array does not contain valid Sensordata!");
            }
            return retVal;
        }

        public static byte[] SDatatoByArrr(Sensordata inputData)
        {
            var protoMsg = new SensorMsg {SequenceNr = inputData.SeqNr};

            //build ProtoBuf Message
            foreach (var tabRow in inputData.DataTable)
            {
                var entry = new DataEntry
                {
                    SensorId = tabRow.Key,
                    Data = tabRow.Value
                };
                protoMsg.DataTable.Add(entry);
            }

            //create string from message
            return protoMsg.ToByteArray();
        }

        public static byte[] RParatoByArr(RegulationParams inputData)
        {
            var protoMsg = new RegParams();

            //assemble message
            if (inputData.ParamD < 0.001 || inputData.ParamD > 0.001)
            {
                protoMsg.ParaD = inputData.ParamD;
            }
            if (inputData.ParamD < 0.001 || inputData.ParamD > 0.001)
            {
                protoMsg.ParaI = inputData.ParamI;
            }
            if (inputData.ParamD < 0.001 || inputData.ParamD > 0.001)
            {
                protoMsg.ParaP = inputData.ParamP;
            }

            protoMsg.TgtVal = inputData.TargetVal;

            switch (inputData.RegTarget)
            {
                case ReguTarget.Angle:
                    protoMsg.Target = 0;
                    break;
                case ReguTarget.Temperature:
                    protoMsg.Target = 1;
                    break;
                case ReguTarget.Velocity:
                    protoMsg.Target = 2;
                    break;
                default:
                    throw new KeyNotFoundException("Wrong Target!");
            }

            //create byte Array from message
            return protoMsg.ToByteArray();
        }
        public static RegulationParams ByArrtoRPara(byte[] inputData)
        {
            var retVal = new RegulationParams();
            //create protoBuf from byte[]
            var protoMsg = RegParams.Parser.ParseFrom(inputData);

            //build parameter class
            retVal.TargetVal = protoMsg.TgtVal;
            retVal.ParamD = protoMsg.ParaD;
            retVal.ParamI = protoMsg.ParaI;
            retVal.ParamP = protoMsg.ParaP;

            switch (protoMsg.Target)
            {
                case 0:
                    retVal.RegTarget = ReguTarget.Angle;
                    break;
                case 1:
                    retVal.RegTarget = ReguTarget.Temperature;
                    break;
                case 2:
                    retVal.RegTarget = ReguTarget.Velocity;
                    break;
                default:
                    throw new KeyNotFoundException("Wrong target!");
            }
            return retVal;
        }
    }
}
