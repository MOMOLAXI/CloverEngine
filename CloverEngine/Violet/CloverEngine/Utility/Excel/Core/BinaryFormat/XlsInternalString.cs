using System;
using System.Text;

namespace Clover
{
    /// <summary>
    /// Plain string without backing storage. Used internally
    /// </summary>
    internal class XlsInternalString : IXlsString
    {
        readonly string m_StringValue;

        public XlsInternalString(string value)
        {
            m_StringValue = value;
        }

        public string GetValue(Encoding encoding)
        {
            return m_StringValue;
        }
    }
}