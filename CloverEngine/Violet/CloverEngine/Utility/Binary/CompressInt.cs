namespace Clover
{
    public static class CompressInt
    {
        public const uint UINT1_MAX = 0x7F;
        public const uint UINT2_MAX = 0x7FFF;
        public const uint UINT3_MAX = 0x7FFFFF;
        public const uint UINT4_MAX = 0x7FFFFFFF;
        public const ulong UINT5_MAX = 0x7FFFFFFFFF;
        public const ulong UINT6_MAX = 0x7FFFFFFFFFFF;
        public const ulong UINT7_MAX = 0x7FFFFFFFFFFFFF;
        public static readonly ulong UINT8_MAX = 0x7FFFFFFFFFFFFFFF;

        public static int WriteInt32(byte[] buf, int startPos, int val)
        {
            if (buf == null || startPos >= buf.Length)
            {
                return 0;
            }

            uint uval = ZigZagEncode32(val);

            if (uval < UINT1_MAX)
            {
                WriteUInt1(buf, startPos, uval);
                return 1;
            }

            if (uval < UINT2_MAX)
            {
                WriteUInt2(buf, startPos, uval);
                return 2;
            }

            if (uval < UINT3_MAX)
            {
                WriteUInt3(buf, startPos, uval);
                return 3;
            }

            WriteUInt4(buf, startPos, uval);
            return 4;
        }

        public static int WriteInt64(byte[] buf, int startPos, long val)
        {
            if (buf == null || startPos >= buf.Length)
            {
                return 0;
            }

            ulong uval = ZigZagEncode64(val);

            if (uval < UINT1_MAX)
            {
                WriteUInt1(buf, startPos, (uint) uval);
                return 1;
            }

            if (uval < UINT2_MAX)
            {
                WriteUInt2(buf, startPos, (uint) uval);
                return 2;
            }

            if (uval < UINT3_MAX)
            {
                WriteUInt3(buf, startPos, (uint) uval);
                return 3;
            }

            if (uval < UINT4_MAX)
            {
                WriteUInt4(buf, startPos, (uint) uval);
                return 4;
            }

            if (uval < UINT5_MAX)
            {
                WriteUInt5(buf, startPos, uval);
                return 5;
            }

            if (uval < UINT6_MAX)
            {
                WriteUInt6(buf, startPos, uval);
                return 6;
            }

            if (uval < UINT7_MAX)
            {
                WriteUInt7(buf, startPos, uval);
                return 7;
            }

            WriteUInt8(buf, startPos, uval);
            return 8;
        }

        public static int ReadInt32(byte[] buf, int startPos, int len)
        {
            uint val = len switch
            {
                1 => ReadUInt1(buf, startPos),
                2 => ReadUInt2(buf, startPos),
                3 => ReadUInt3(buf, startPos),
                _ => ReadUInt4(buf, startPos)
            };

            return ZigZagDecode32(val);
        }

        public static long ReadInt64(byte[] buf, int startPos, int len)
        {
            ulong val = len switch
            {
                1 => ReadUInt1(buf, startPos),
                2 => ReadUInt2(buf, startPos),
                3 => ReadUInt3(buf, startPos),
                4 => ReadUInt4(buf, startPos),
                5 => ReadUInt5(buf, startPos),
                6 => ReadUInt6(buf, startPos),
                7 => ReadUInt7(buf, startPos),
                _ => ReadUInt8(buf, startPos)
            };

            return ZigZagDecode64(val);
        }

        static void WriteUInt1(byte[] buf, int startPos, uint val)
        {
            if (startPos < buf.Length)
            {
                buf[startPos] = (byte) val;
            }
        }

        static void WriteUInt2(byte[] buf, int startPos, uint val)
        {
            if (startPos + 1 >= buf.Length)
            {
                return;
            }

            buf[startPos] = (byte) ((val >> 8) & 0xFF);
            buf[startPos + 1] = (byte) (val & 0xFF);
        }

        static void WriteUInt3(byte[] buf, int startPos, uint val)
        {
            if (startPos + 2 >= buf.Length)
            {
                return;
            }

            buf[startPos] = (byte) ((val >> 16) & 0xFF);
            buf[startPos + 1] = (byte) ((val >> 8) & 0xFF);
            buf[startPos + 2] = (byte) (val & 0xFF);
        }

        static void WriteUInt4(byte[] buf, int startPos, uint val)
        {
            if (startPos + 3 >= buf.Length)
            {
                return;
            }

            buf[startPos] = (byte) ((val >> 24) & 0xFF);
            buf[startPos + 1] = (byte) ((val >> 16) & 0xFF);
            buf[startPos + 2] = (byte) ((val >> 8) & 0xFF);
            buf[startPos + 3] = (byte) (val & 0xFF);
        }

        static void WriteUInt5(byte[] buf, int startPos, ulong val)
        {
            if (startPos + 4 >= buf.Length)
            {
                return;
            }

            buf[startPos] = (byte) ((val >> 32) & 0xFF);
            buf[startPos + 1] = (byte) ((val >> 24) & 0xFF);
            buf[startPos + 2] = (byte) ((val >> 16) & 0xFF);
            buf[startPos + 3] = (byte) ((val >> 8) & 0xFF);
            buf[startPos + 4] = (byte) (val & 0xFF);
        }

        static void WriteUInt6(byte[] buf, int startPos, ulong val)
        {
            if (startPos + 5 >= buf.Length)
            {
                return;
            }

            buf[startPos] = (byte) ((val >> 40) & 0xFF);
            buf[startPos + 1] = (byte) ((val >> 32) & 0xFF);
            buf[startPos + 2] = (byte) ((val >> 24) & 0xFF);
            buf[startPos + 3] = (byte) ((val >> 16) & 0xFF);
            buf[startPos + 4] = (byte) ((val >> 8) & 0xFF);
            buf[startPos + 5] = (byte) (val & 0xFF);
        }

        static void WriteUInt7(byte[] buf, int startPos, ulong val)
        {
            if (startPos + 6 >= buf.Length)
            {
                return;
            }

            buf[startPos] = (byte) ((val >> 48) & 0xFF);
            buf[startPos + 1] = (byte) ((val >> 40) & 0xFF);
            buf[startPos + 2] = (byte) ((val >> 32) & 0xFF);
            buf[startPos + 3] = (byte) ((val >> 24) & 0xFF);
            buf[startPos + 4] = (byte) ((val >> 16) & 0xFF);
            buf[startPos + 5] = (byte) ((val >> 8) & 0xFF);
            buf[startPos + 6] = (byte) (val & 0xFF);
        }

        static void WriteUInt8(byte[] buf, int startPos, ulong val)
        {
            if (startPos + 7 >= buf.Length)
            {
                return;
            }

            buf[startPos] = (byte) ((val >> 54) & 0xFF);
            buf[startPos + 1] = (byte) ((val >> 48) & 0xFF);
            buf[startPos + 2] = (byte) ((val >> 40) & 0xFF);
            buf[startPos + 3] = (byte) ((val >> 32) & 0xFF);
            buf[startPos + 4] = (byte) ((val >> 24) & 0xFF);
            buf[startPos + 5] = (byte) ((val >> 16) & 0xFF);
            buf[startPos + 6] = (byte) ((val >> 8) & 0xFF);
            buf[startPos + 7] = (byte) (val & 0xFF);
        }

        static uint ReadUInt1(byte[] buf, int startPos)
        {
            return startPos < buf.Length ? buf[startPos] : (uint) 0;
        }

        static uint ReadUInt2(byte[] buf, int startPos)
        {
            if (startPos + 1 >= buf.Length)
            {
                return 0;
            }

            uint val = 0;

            val |= (uint) buf[startPos] << 8;
            val |= buf[startPos + 1];

            return val;
        }

        static uint ReadUInt3(byte[] buf, int startPos)
        {
            if (startPos + 2 >= buf.Length)
            {
                return 0;
            }

            uint val = 0;

            val |= (uint) buf[startPos] << 16;
            val |= (uint) buf[startPos + 1] << 8;
            val |= buf[startPos + 2];

            return val;
        }

        static uint ReadUInt4(byte[] buf, int startPos)
        {
            if (startPos + 3 >= buf.Length)
            {
                return 0;
            }

            uint val = 0;

            val |= (uint) buf[startPos] << 24;
            val |= (uint) buf[startPos + 1] << 16;
            val |= (uint) buf[startPos + 2] << 8;
            val |= buf[startPos + 3];

            return val;
        }

        static ulong ReadUInt5(byte[] buf, int startPos)
        {
            if (startPos + 4 >= buf.Length)
            {
                return 0;
            }

            ulong val = 0;

            val |= (ulong) buf[startPos] << 32;
            val |= (ulong) buf[startPos + 1] << 24;
            val |= (ulong) buf[startPos + 2] << 16;
            val |= (ulong) buf[startPos + 3] << 8;
            val |= buf[startPos + 4];

            return val;
        }

        static ulong ReadUInt6(byte[] buf, int startPos)
        {
            if (startPos + 5 >= buf.Length)
            {
                return 0;
            }

            ulong val = 0;

            val |= (ulong) buf[startPos] << 40;
            val |= (ulong) buf[startPos + 1] << 32;
            val |= (ulong) buf[startPos + 2] << 24;
            val |= (ulong) buf[startPos + 3] << 16;
            val |= (ulong) buf[startPos + 4] << 8;
            val |= buf[startPos + 5];

            return val;
        }

        static ulong ReadUInt7(byte[] buf, int startPos)
        {
            if (startPos + 6 >= buf.Length)
            {
                return 0;
            }

            ulong val = 0;

            val |= (ulong) buf[startPos] << 48;
            val |= (ulong) buf[startPos + 1] << 40;
            val |= (ulong) buf[startPos + 2] << 32;
            val |= (ulong) buf[startPos + 3] << 24;
            val |= (ulong) buf[startPos + 4] << 16;
            val |= (ulong) buf[startPos + 5] << 8;
            val |= buf[startPos + 6];

            return val;
        }

        static ulong ReadUInt8(byte[] buf, int startPos)
        {
            if (startPos + 7 >= buf.Length)
            {
                return 0;
            }

            ulong val = 0;

            val |= (ulong) buf[startPos] << 56;
            val |= (ulong) buf[startPos + 1] << 48;
            val |= (ulong) buf[startPos + 2] << 40;
            val |= (ulong) buf[startPos + 3] << 32;
            val |= (ulong) buf[startPos + 4] << 24;
            val |= (ulong) buf[startPos + 5] << 16;
            val |= (ulong) buf[startPos + 6] << 8;
            val |= buf[startPos + 7];

            return val;
        }

        // ZigZag Transform:  Encodes signed integers so that they can be
        // effectively used with varint encoding.
        //
        // varint operates on unsigned integers, encoding smaller numbers into
        // fewer bytes.  If you try to use it on a signed integer, it will treat
        // this number as a very large unsigned integer, which means that even
        // small signed numbers like -1 will take the maximum number of bytes
        // (10) to encode.  ZigZagEncode() maps signed integers to unsigned
        // in such a way that those with a small absolute Value will have smaller
        // encoded values, making them appropriate for encoding using varint.
        //
        //       int32 ->     uint32
        // -------------------------
        //           0 ->          0
        //          -1 ->          1
        //           1 ->          2
        //          -2 ->          3
        //         ... ->        ...
        //  2147483647 -> 4294967294
        // -2147483648 -> 4294967295
        //
        //        >> encode >>
        //        << decode <<
        static uint ZigZagEncode32(int n)
        {
            return (uint) ((n << 1) ^ (n >> 31));
        }

        static int ZigZagDecode32(uint n)
        {
            return (int) ((n >> 1) ^ -(int) (n & 1));
        }

        static ulong ZigZagEncode64(long n)
        {
            return (ulong) ((n << 1) ^ (n >> 63));
        }

        static long ZigZagDecode64(ulong n)
        {
            return (long) (n >> 1) ^ -(long) (n & 1);
        }
    }
}