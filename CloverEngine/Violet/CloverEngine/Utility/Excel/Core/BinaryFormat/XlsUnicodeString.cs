using System;
using System.Text;

namespace Clover
{
    /// <summary>
    /// [MS-XLS] 2.5.294 XLUnicodeString
    /// Word-sized string, stored as single or multibyte unicode characters.
    /// </summary>
    internal class XlsUnicodeString : IXlsString
    {
        readonly byte[] m_Bytes;
        readonly uint m_Offset;

        public XlsUnicodeString(byte[] bytes, uint offset)
        {
            m_Bytes = bytes;
            m_Offset = offset;
        }

        public ushort CharacterCount => BitConverter.ToUInt16(m_Bytes, (int) m_Offset);

        /// <summary>
        /// Gets a value indicating whether the string is a multibyte string or not.
        /// </summary>
        public bool IsMultiByte => (m_Bytes[m_Offset + 2] & 0x01) != 0;

        public string GetValue(Encoding encoding)
        {
            if (CharacterCount == 0)
            {
                return string.Empty;
            }

            if (IsMultiByte)
            {
                return Encoding.Unicode.GetString(m_Bytes, (int) m_Offset + 3, CharacterCount * 2);
            }

            byte[] bytes = new byte[CharacterCount * 2];
            for (int i = 0; i < CharacterCount; i++)
            {
                bytes[i * 2] = m_Bytes[m_Offset + 3 + i];
            }

            return Encoding.Unicode.GetString(bytes);
        }
    }
}