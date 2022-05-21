using System;
using System.Text;
using Clover;

namespace Clover
{
    /// <summary>
    /// Word-sized string, stored as single bytes with encoding from CodePage record. Used in BIFF2-5 
    /// </summary>
    internal class XlsByteString : IXlsString
    {
        readonly byte[] m_Bytes;
        readonly uint m_Offset;

        public XlsByteString(byte[] bytes, uint offset)
        {
            m_Bytes = bytes;
            m_Offset = offset;
        }

        /// <summary>
        /// Gets the number of characters in the string.
        /// </summary>
        public ushort CharacterCount => BitConverter.ToUInt16(m_Bytes, (int) m_Offset);

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string GetValue(Encoding encoding)
        {
            byte[] stringBytes = ReadArray(0x2, CharacterCount * (Helpers.IsSingleByteEncoding(encoding) ? 1 : 2));
            return encoding.GetString(stringBytes, 0, stringBytes.Length);
        }

        public byte[] ReadArray(int offset, int size)
        {
            byte[] tmp = new byte[size];
            Buffer.BlockCopy(m_Bytes, (int) (m_Offset + offset), tmp, 0, size);
            return tmp;
        }
    }
}