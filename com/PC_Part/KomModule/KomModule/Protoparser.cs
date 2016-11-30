using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;
using MotorXP.Protobuf.SensorMSg;
using MotorXP.Protobuf.ParamMSg;

namespace KomModule
{
    public class Protoparser
    {
        public static Sensordata ByArrtoSData(byte[] InputData)
        {
            Sensordata retVal = new Sensordata();
            SensorMsg protoMsg;

            retVal.DataTable = new SortedList<ushort, Double>();

            //create ProtoBuf Message from byteArray
            try
            {
                protoMsg = SensorMsg.Parser.ParseFrom(InputData);
                //build Sensordata object
                retVal.SeqNr = protoMsg.SequenceNr;
                for (int i = 0; i < protoMsg.DataTable.Count; i++)
                {
                    retVal.DataTable.Add((UInt16)protoMsg.DataTable[i].SensorId, protoMsg.DataTable[i].Data);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Input Array does not contain valid Sensordata!");
            }
            return retVal;
        }

        public static byte[] SDatatoByArrr(Sensordata InputData)
        {
            SensorMsg protoMsg = new SensorMsg();

            //build ProtoBuf Message
            protoMsg.SequenceNr = InputData.SeqNr;
            foreach (var tabRow in InputData.DataTable)
            {
                DataEntry entry = new DataEntry();
                entry.SensorId = tabRow.Key;
                entry.Data = tabRow.Value;
                protoMsg.DataTable.Add(entry);
            }

            //create string from message
            return protoMsg.ToByteArray();
        }

        public static byte[] RParatoByArr(RegulationParams InputData)
        {
            RegParams protoMsg = new RegParams();

            //assemble message
            if (InputData.ParamD < 0.001 || InputData.ParamD > 0.001)
            {
                protoMsg.ParaD = InputData.ParamD;
            }
            if (InputData.ParamD < 0.001 || InputData.ParamD > 0.001)
            {
                protoMsg.ParaI = InputData.ParamI;
            }
            if (InputData.ParamD < 0.001 || InputData.ParamD > 0.001)
            {
                protoMsg.ParaP = InputData.ParamP;
            }

            protoMsg.TgtVal = InputData.TargetVal;

            switch (InputData.RegTarget)
            {
                case reguTarget.ANGLE:
                    protoMsg.Target = 0;
                    break;
                case reguTarget.TEMPERATURE:
                    protoMsg.Target = 1;
                    break;
                case reguTarget.VELOCITY:
                    protoMsg.Target = 2;
                    break;
                default:
                    throw new KeyNotFoundException("Wrong Target!");
            }

            //create byte Array from message
            return protoMsg.ToByteArray();
        }
        public static RegulationParams ByArrtoRPara(byte[] InputData)
        {
            RegParams protoMsg;
            RegulationParams retVal = new RegulationParams();
            //create protoBuf from byte[]
            protoMsg = RegParams.Parser.ParseFrom(InputData);

            //build parameter class
            retVal.TargetVal = protoMsg.TgtVal;
            retVal.ParamD = protoMsg.ParaD;
            retVal.ParamI = protoMsg.ParaI;
            retVal.ParamP = protoMsg.ParaP;

            switch (protoMsg.Target)
            {
                case 0:
                    retVal.RegTarget = reguTarget.ANGLE;
                    break;
                case 1:
                    retVal.RegTarget = reguTarget.TEMPERATURE;
                    break;
                case 2:
                    retVal.RegTarget = reguTarget.VELOCITY;
                    break;
                default:
                    throw new KeyNotFoundException("Wrong target!");
            }
            return retVal;
        }
    }
}
