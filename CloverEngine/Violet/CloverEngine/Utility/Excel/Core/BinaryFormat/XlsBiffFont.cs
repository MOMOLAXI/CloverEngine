using System.Text;
using Clover;

namespace Clover
{
    /// <summary>
    /// The font with index 4 is omitted in all BIFF versions. This means the first four fonts have zero-based indexes, and the fifth font and all following fonts are referenced with one-based indexes.
    /// </summary>
    internal class XlsBiffFont : XlsBiffRecord
    {
        readonly IXlsString m_FontName;

        internal XlsBiffFont(byte[] bytes, int biffVersion)
            : base(bytes)
        {
            m_FontName = Id switch
            {
                BIFFRECORDTYPE.FONT_V34 => new XlsShortByteString(bytes, ContentOffset + 6),
                BIFFRECORDTYPE.FONT when biffVersion == 2 => new XlsShortByteString(bytes, ContentOffset + 4),
                BIFFRECORDTYPE.FONT when biffVersion == 5 => new XlsShortByteString(bytes, ContentOffset + 14),
                BIFFRECORDTYPE.FONT when biffVersion == 8 => new XlsShortUnicodeString(bytes, ContentOffset + 14),
                _ => new XlsInternalString(string.Empty)
            };

            switch (Id)
            {
                case BIFFRECORDTYPE.FONT when biffVersion >= 5:
                {
                    // Encodings were mapped by correlating this:
                    // https://docs.microsoft.com/en-us/windows/desktop/intl/code-page-identifiers
                    // with the FONT record character set table here:
                    // https://www.openoffice.org/sc/excelfileformat.pdf
                    byte byteStringCharacterSet = ReadByte(12);
                    switch (byteStringCharacterSet)
                    {
                        case 0: // ANSI Latin
                        case 1: // System default
                            ByteStringEncoding = Helper.GetEncoding(1252);
                            break;
                        case 77: // Apple roman
                            ByteStringEncoding = Helper.GetEncoding(10000);
                            break;
                        case 128: // ANSI Japanese Shift-JIS
                            ByteStringEncoding = Helper.GetEncoding(932);
                            break;
                        case 129: // ANSI Korean (Hangul)
                            ByteStringEncoding = Helper.GetEncoding(949);
                            break;
                        case 130: // ANSI Korean (Johab)
                            ByteStringEncoding = Helper.GetEncoding(1361);
                            break;
                        case 134: // ANSI Chinese Simplified GBK
                            ByteStringEncoding = Helper.GetEncoding(936);
                            break;
                        case 136: // ANSI Chinese Traditional BIG5
                            ByteStringEncoding = Helper.GetEncoding(950);
                            break;
                        case 161: // ANSI Greek
                            ByteStringEncoding = Helper.GetEncoding(1253);
                            break;
                        case 162: // ANSI Turkish
                            ByteStringEncoding = Helper.GetEncoding(1254);
                            break;
                        case 163: // ANSI Vietnamese
                            ByteStringEncoding = Helper.GetEncoding(1258);
                            break;
                        case 177: // ANSI Hebrew
                            ByteStringEncoding = Helper.GetEncoding(1255);
                            break;
                        case 178: // ANSI Arabic
                            ByteStringEncoding = Helper.GetEncoding(1256);
                            break;
                        case 186: // ANSI Baltic
                            ByteStringEncoding = Helper.GetEncoding(1257);
                            break;
                        case 204: // ANSI Cyrillic
                            ByteStringEncoding = Helper.GetEncoding(1251);
                            break;
                        case 222: // ANSI Thai
                            ByteStringEncoding = Helper.GetEncoding(874);
                            break;
                        case 238: // ANSI Latin II
                            ByteStringEncoding = Helper.GetEncoding(1250);
                            break;
                        case 255: // OEM Latin
                            ByteStringEncoding = Helper.GetEncoding(850);
                            break;
                    }

                    break;
                }
            }
        }

        public Encoding ByteStringEncoding { get; }

        public string GetFontName(Encoding encoding) => m_FontName.GetValue(encoding);
    }
}