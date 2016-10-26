using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;
using MotorXP.Protobuf.SensorMSg;

namespace KomModule
{
    class Protoparser
    {
        internal static Sensordata ByArrtoSData(byte[] InputData)
        {
            Sensordata retVal = new Sensordata();
            SensorMsg protoMsg;

            //create ProtoBuf Message from byteArray
            protoMsg = SensorMsg.Parser.ParseFrom(InputData);
            //build Sensordata object
            retVal.Timestamp = protoMsg.SequenceNr;
            for(int i = 0; i < protoMsg.DataTable.Count; i++)
            {
                retVal.DataTable.Add((UInt16)protoMsg.DataTable[i].SensorId, protoMsg.DataTable[i].Data);
            }
            return retVal;
        }

        internal static string SDatatoString(Sensordata InputData)
        {
            string retval;
            SensorMsg protoMsg = new SensorMsg();
            UnicodeEncoding enc = new UnicodeEncoding();

            //build ProtoBuf Message
            protoMsg.SequenceNr = InputData.Timestamp;
            foreach (var tabRow in InputData.DataTable)
            {
                DataEntry entry = new DataEntry();
                entry.SensorId = tabRow.Key;
                entry.Data = tabRow.Value;
                protoMsg.DataTable.Add(entry);
            }

            //create string from message
            byte[] bArr = protoMsg.ToByteArray();
            retval = System.Text.Encoding.Unicode.GetString(bArr);

            return retval;
        }
    }
}
