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
            byte[] retVal = new byte[InputData.Length - 2];
            byte[] crc = crcGen.ComputeChecksumBytes(InputData);
            if(crc[0] == corrCrc[0] && crc[1] == corrCrc[1])
            {
                //correct crc
                for (int i = 0; i < retVal.Length; i++)
                {
                    retVal[i] = InputData[i];
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
            Int32 length = OutputData.Length;
            byte[] retVal = new byte[length + 2];
            UInt16 crc = crcGen.ComputeChecksum(OutputData);
            for (int i = 0; i < length; i++)
            {
                retVal[i] = OutputData[i];
            }
            retVal[length] = (byte)(crc >> 8);
            retVal[length + 1] = (byte)crc;

            return retVal;
        }
    }
}
