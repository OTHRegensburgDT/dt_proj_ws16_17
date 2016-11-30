using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomModule
{
    public class Frameparser
    {
        private static Crc16Ccitt crcGen = new Crc16Ccitt(InitialCrcValue.NonZero1);
        public static byte[] DecapsuleFrame(byte[] InputData)
        {
            byte[] corrCrc = { 0, 0 };
            byte[] tmp = new byte[InputData.Length - 1];
            byte[] retVal = new byte[InputData.Length - 3];
            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = InputData[i + 1];
            }
            byte[] crc = crcGen.ComputeChecksumBytes(tmp);
            if(crc[0] == corrCrc[0] && crc[1] == corrCrc[1])
            {
                //correct crc
                for (int i = 0; i < retVal.Length; i++)
                {
                    retVal[i] = tmp[i];
                }
            }
            else
            {
                retVal = new byte[1];
                retVal[0] = 0;
            }
                return retVal;
        }
        public static byte[] EncapsuleFrame(byte[] OutputData)
        {
            byte length = (byte)OutputData.Length;
            byte[] retVal = new byte[length + 3];
            UInt16 crc = crcGen.ComputeChecksum(OutputData);
            for (int i = 0; i < length; i++)
            {
                retVal[i+1] = OutputData[i];
            }
            retVal[length + 1] = (byte)(crc >> 8);
            retVal[length + 2] = (byte)crc;
            retVal[0] = (byte)(length + 3);
            return retVal;
        }
    }
}
