using System;
using System.Text;
using Clover;

namespace Clover
{
    /// <summary>
    /// Byte sized string, stored as bytes, with encoding from CodePage record. Used in BIFF2-5 .
    /// </summary>
    internal class XlsShortByteString : IXlsString
    {
        readonly byte[] m_Bytes;
        readonly uint m_Offset;

        public XlsShortByteString(byte[] bytes, uint offset)
        {
            m_Bytes = bytes;
            m_Offset = offset;
        }

        public ushort CharacterCount => m_Bytes[m_Offset];

        public string GetValue(Encoding encoding)
        {
            // Supposedly this is never multibyte, but technically could be
            if (!Helpers.IsSingleByteEncoding(encoding))
            {
                return encoding.GetString(m_Bytes, (int) m_Offset + 1, CharacterCount * 2);
            }

            return encoding.GetString(m_Bytes, (int) m_Offset + 1, CharacterCount);
        }
    }
}