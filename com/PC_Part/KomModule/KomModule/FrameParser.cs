namespace KomModule
{
    public static class Frameparser
    {
        private static readonly Crc16Ccitt CrcGen = new Crc16Ccitt(InitialCrcValue.NonZero1);
        public static byte[] DecapsuleFrame(byte[] inputData)
        {
            byte[] corrCrc = { 0, 0 };
            var tmp = new byte[inputData.Length - 1];
            var retVal = new byte[inputData.Length - 3];
            for (var i = 0; i < tmp.Length; i++)
            {
                tmp[i] = inputData[i + 1];
            }
            var crc = CrcGen.ComputeChecksumBytes(tmp);
            if(crc[0] == corrCrc[0] && crc[1] == corrCrc[1])
            {
                //correct crc
                for (var i = 0; i < retVal.Length; i++)
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
        public static byte[] EncapsuleFrame(byte[] outputData)
        {
            var length = (byte)outputData.Length;
            var retVal = new byte[length + 3];
            var crc = CrcGen.ComputeChecksum(outputData);
            for (var i = 0; i < length; i++)
            {
                retVal[i+1] = outputData[i];
            }
            retVal[length + 1] = (byte)(crc >> 8);
            retVal[length + 2] = (byte)crc;
            retVal[0] = (byte)(length + 3);
            return retVal;
        }
    }
}
