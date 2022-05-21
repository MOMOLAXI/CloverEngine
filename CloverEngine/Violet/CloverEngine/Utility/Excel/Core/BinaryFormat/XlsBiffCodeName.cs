using System.Text;

namespace Clover
{
    internal class XlsBiffCodeName : XlsBiffRecord
    {
        readonly IXlsString m_XlsString;

        internal XlsBiffCodeName(byte[] bytes)
            : base(bytes)
        {
            // BIFF8 only
            m_XlsString = new XlsUnicodeString(bytes, ContentOffset);
        }

        public string GetValue(Encoding encoding)
        {
            return m_XlsString.GetValue(encoding);
        }
    }
}