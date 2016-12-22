using System;

namespace KomModule
{
    public enum InitialCrcValue { Zeros, NonZero1 = 0xffff, NonZero2 = 0x1D0F }

    public class Crc16Ccitt
    {
        private const ushort Poly = 4129;
        private readonly ushort[] _table = new ushort[256];
        private readonly ushort _initialValue;

        public ushort ComputeChecksum(byte[] bytes)
        {
            var crc = _initialValue;
            foreach (var bit in bytes)
            {
                crc = (ushort)((crc << 8) ^ _table[(crc >> 8) ^ (0xff & bit)]);
            }
            return crc;
        }

        public byte[] ComputeChecksumBytes(byte[] bytes)
        {
            var crc = ComputeChecksum(bytes);
            return BitConverter.GetBytes(crc);
        }

        public Crc16Ccitt(InitialCrcValue initialValue)
        {
            _initialValue = (ushort)initialValue;
            for (var i = 0; i < _table.Length; ++i)
            {
                ushort temp = 0;
                var a = (ushort)(i << 8);
                for (var j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                    {
                        temp = (ushort)((temp << 1) ^ Poly);
                    }
                    else
                    {
                        temp <<= 1;
                    }
                    a <<= 1;
                }
                _table[i] = temp;
            }
        }
    }
}