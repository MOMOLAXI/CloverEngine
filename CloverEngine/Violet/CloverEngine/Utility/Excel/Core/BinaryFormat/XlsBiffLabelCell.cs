using System;
using System.Text;

namespace Clover
{
    /// <summary>
    /// [MS-XLS] 2.4.148 Label
    /// Represents a string
    /// </summary>
    internal class XlsBiffLabelCell : XlsBiffBlankCell
    {
        readonly IXlsString m_XlsString;

        internal XlsBiffLabelCell(byte[] bytes, int biffVersion)
            : base(bytes)
        {
            if (Id == BIFFRECORDTYPE.LABEL_OLD)
            {
                // BIFF2
                m_XlsString = new XlsShortByteString(bytes, ContentOffset + 7);
            }
            else if (biffVersion >= 2 && biffVersion <= 5)
            {
                // BIFF3-5, or if there is a newer label record present in a BIFF2 stream
                m_XlsString = new XlsByteString(bytes, ContentOffset + 6);
            }
            else if (biffVersion == 8)
            {
                // BIFF8
                m_XlsString = new XlsUnicodeString(bytes, ContentOffset + 6);
            }
            else
            {
                throw new ArgumentException("Unexpected BIFF version " + biffVersion, nameof(biffVersion));
            }
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        public string GetValue(Encoding encoding)
        {
            return m_XlsString.GetValue(encoding);
        }
    }
}