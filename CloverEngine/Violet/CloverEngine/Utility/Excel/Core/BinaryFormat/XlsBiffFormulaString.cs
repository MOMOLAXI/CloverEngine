using System;
using System.Text;

namespace Clover
{
    /// <summary>
    /// Represents a string value of formula
    /// </summary>
    internal class XlsBiffFormulaString : XlsBiffRecord
    {
        readonly IXlsString m_XlsString;

        internal XlsBiffFormulaString(byte[] bytes, int biffVersion)
            : base(bytes)
        {
            switch (biffVersion)
            {
                case 2:
                    // BIFF2
                    m_XlsString = new XlsShortByteString(bytes, ContentOffset);
                    break;
                default:
                {
                    if (biffVersion >= 3 && biffVersion <= 5)
                    {
                        // BIFF3-5
                        m_XlsString = new XlsByteString(bytes, ContentOffset);
                    }
                    else if (biffVersion == 8)
                    {
                        // BIFF8
                        m_XlsString = new XlsUnicodeString(bytes, ContentOffset);
                    }
                    else
                    {
                        throw new ArgumentException("Unexpected BIFF version " + biffVersion, nameof(biffVersion));
                    }

                    break;
                }
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