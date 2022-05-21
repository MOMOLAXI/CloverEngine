using System;
using System.Text;

namespace Clover
{
    /// <summary>
    /// Represents a string value of format
    /// </summary>
    internal class XlsBiffFormatString : XlsBiffRecord
    {
        readonly IXlsString m_XlsString;

        internal XlsBiffFormatString(byte[] bytes, int biffVersion)
            : base(bytes)
        {
            if (Id == BIFFRECORDTYPE.FORMAT_V23)
            {
                // BIFF2-3
                m_XlsString = new XlsShortByteString(bytes, ContentOffset);
            }
            else if (biffVersion >= 2 && biffVersion <= 5)
            {
                // BIFF4-5, or if there is a newer format record in a BIFF2-3 stream
                m_XlsString = new XlsShortByteString(bytes, ContentOffset + 2);
            }
            else if (biffVersion == 8)
            {
                // BIFF8
                m_XlsString = new XlsUnicodeString(bytes, ContentOffset + 2);
            }
            else
            {
                throw new ArgumentException("Unexpected BIFF version " + biffVersion, nameof(biffVersion));
            }
        }

        public ushort Index
        {
            get
            {
                return Id switch
                {
                    BIFFRECORDTYPE.FORMAT_V23 => throw new NotSupportedException(
                        "Index is not available for BIFF2 and BIFF3 FORMAT records."),
                    _ => ReadUInt16(0)
                };
            }
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        public string GetValue(Encoding encoding)
        {
            return m_XlsString.GetValue(encoding);
        }
    }
}