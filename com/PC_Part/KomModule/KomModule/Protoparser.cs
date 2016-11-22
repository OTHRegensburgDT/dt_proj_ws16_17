using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;
using MotorXP.Protobuf.SensorMSg;

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
            protoMsg = SensorMsg.Parser.ParseFrom(InputData);
            //build Sensordata object
            retVal.SeqNr = protoMsg.SequenceNr;
            for(int i = 0; i < protoMsg.DataTable.Count; i++)
            {
                retVal.DataTable.Add((UInt16)protoMsg.DataTable[i].SensorId, protoMsg.DataTable[i].Data);
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
    }
}
