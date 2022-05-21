using System;
using System.Text;

namespace Clover
{
    /// <summary>
    /// [MS-XLS] 2.5.240 ShortXLUnicodeString
    /// Byte-sized string, stored as single or multibyte unicode characters.
    /// </summary>
    internal class XlsShortUnicodeString : IXlsString
    {
        readonly byte[] m_Bytes;
        readonly uint m_Offset;

        public XlsShortUnicodeString(byte[] bytes, uint offset)
        {
            m_Bytes = bytes;
            m_Offset = offset;
        }

        public ushort CharacterCount => m_Bytes[m_Offset];

        /// <summary>
        /// Gets a value indicating whether the string is a multibyte string or not.
        /// </summary>
        public bool IsMultiByte => (m_Bytes[m_Offset + 1] & 0x01) != 0;

        public string GetValue(Encoding encoding)
        {
            if (CharacterCount == 0)
            {
                return string.Empty;
            }

            if (IsMultiByte)
            {
                return Encoding.Unicode.GetString(m_Bytes, (int) m_Offset + 2, CharacterCount * 2);
            }

            byte[] bytes = new byte[CharacterCount * 2];
            for (int i = 0; i < CharacterCount; i++)
            {
                bytes[i * 2] = m_Bytes[m_Offset + 2 + i];
            }

            return Encoding.Unicode.GetString(bytes);
        }
    }
}