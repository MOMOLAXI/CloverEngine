using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Clover
{
    public static partial class Helper
    {
        public static string SafeFormat(this string format, params object[] args)
        {
            string ret = format;

            try
            {
                ret = string.Format(format, args);
            }
            catch (Exception ex)
            {
                Log.InternalError(ex.Message + ex.StackTrace);
            }

            return ret;
        }

        public static string BytesToString(byte[] buffer)
        {
            if (buffer == null)
            {
                return string.Empty;
            }

            string res = string.Empty;

            foreach (byte b in buffer)
            {
                res += string.Format("{0:X}", b / 16);
                res += string.Format("{0:X}", b % 16);
            }

            return res;
        }

        public static bool ToEnum<T>(string value, out T result) where T : struct, Enum
        {
            return value.ParseEnum(out result);
        }

        public static int ToInt(string value)
        {
            return value.ParseInt();
        }

        public static bool ToBool(string value)
        {
            return value.ParseBool();
        }

        public static long ToInt64(string value)
        {
            return value.ParseInt64();
        }

        public static float ToFloat(string value)
        {
            return value.ParseFloat();
        }

        public static Encoding GetEncoding(ushort codePage)
        {
            Encoding encoding = codePage switch
            {
                037 => Encoding.GetEncoding("IBM037"),
                437 => Encoding.GetEncoding("IBM437"),
                500 => Encoding.GetEncoding("IBM500"),
                708 => Encoding.GetEncoding("ASMO-708"),
                709 => Encoding.GetEncoding(string.Empty),
                710 => Encoding.GetEncoding(string.Empty),
                720 => Encoding.GetEncoding("DOS-720"),
                737 => Encoding.GetEncoding("ibm737"),
                775 => Encoding.GetEncoding("ibm775"),
                850 => Encoding.GetEncoding("ibm850"),
                852 => Encoding.GetEncoding("ibm852"),
                855 => Encoding.GetEncoding("IBM855"),
                857 => Encoding.GetEncoding("ibm857"),
                858 => Encoding.GetEncoding("IBM00858"),
                860 => Encoding.GetEncoding("IBM860"),
                861 => Encoding.GetEncoding("ibm861"),
                862 => Encoding.GetEncoding("DOS-862"),
                863 => Encoding.GetEncoding("IBM863"),
                864 => Encoding.GetEncoding("IBM864"),
                865 => Encoding.GetEncoding("IBM865"),
                866 => Encoding.GetEncoding("cp866"),
                869 => Encoding.GetEncoding("ibm869"),
                870 => Encoding.GetEncoding("IBM870"),
                874 => Encoding.GetEncoding("windows-874"),
                875 => Encoding.GetEncoding("cp875"),
                932 => Encoding.GetEncoding("shift_jis"),
                936 => Encoding.GetEncoding("gb2312"),
                949 => Encoding.GetEncoding("ks_c_5601-1987"),
                950 => Encoding.GetEncoding("big5"),
                1026 => Encoding.GetEncoding("IBM1026"),
                1047 => Encoding.GetEncoding("IBM01047"),
                1140 => Encoding.GetEncoding("IBM01140"),
                1141 => Encoding.GetEncoding("IBM01141"),
                1142 => Encoding.GetEncoding("IBM01142"),
                1143 => Encoding.GetEncoding("IBM01143"),
                1144 => Encoding.GetEncoding("IBM01144"),
                1145 => Encoding.GetEncoding("IBM01145"),
                1146 => Encoding.GetEncoding("IBM01146"),
                1147 => Encoding.GetEncoding("IBM01147"),
                1148 => Encoding.GetEncoding("IBM01148"),
                1149 => Encoding.GetEncoding("IBM01149"),
                1200 => Encoding.GetEncoding("utf-16"),
                1201 => Encoding.GetEncoding("unicodeFFFE"),
                1250 => Encoding.GetEncoding("windows-1250"),
                1251 => Encoding.GetEncoding("windows-1251"),
                1252 => Encoding.GetEncoding("windows-1252"),
                1253 => Encoding.GetEncoding("windows-1253"),
                1254 => Encoding.GetEncoding("windows-1254"),
                1255 => Encoding.GetEncoding("windows-1255"),
                1256 => Encoding.GetEncoding("windows-1256"),
                1257 => Encoding.GetEncoding("windows-1257"),
                1258 => Encoding.GetEncoding("windows-1258"),
                1361 => Encoding.GetEncoding("Johab"),
                10000 => Encoding.GetEncoding("macintosh"),
                10001 => Encoding.GetEncoding("x-mac-japanese"),
                10002 => Encoding.GetEncoding("x-mac-chinesetrad"),
                10003 => Encoding.GetEncoding("x-mac-korean"),
                10004 => Encoding.GetEncoding("x-mac-arabic"),
                10005 => Encoding.GetEncoding("x-mac-hebrew"),
                10006 => Encoding.GetEncoding("x-mac-greek"),
                10007 => Encoding.GetEncoding("x-mac-cyrillic"),
                10008 => Encoding.GetEncoding("x-mac-chinesesimp"),
                10010 => Encoding.GetEncoding("x-mac-romanian"),
                10017 => Encoding.GetEncoding("x-mac-ukrainian"),
                10021 => Encoding.GetEncoding("x-mac-thai"),
                10029 => Encoding.GetEncoding("x-mac-ce"),
                10079 => Encoding.GetEncoding("x-mac-icelandic"),
                10081 => Encoding.GetEncoding("x-mac-turkish"),
                10082 => Encoding.GetEncoding("x-mac-croatian"),
                12000 => Encoding.GetEncoding("utf-32"),
                12001 => Encoding.GetEncoding("utf-32BE"),
                20000 => Encoding.GetEncoding("x-Chinese_CNS"),
                20001 => Encoding.GetEncoding("x-cp20001"),
                20002 => Encoding.GetEncoding("x_Chinese-Eten"),
                20003 => Encoding.GetEncoding("x-cp20003"),
                20004 => Encoding.GetEncoding("x-cp20004"),
                20005 => Encoding.GetEncoding("x-cp20005"),
                20105 => Encoding.GetEncoding("x-IA5"),
                20106 => Encoding.GetEncoding("x-IA5-German"),
                20107 => Encoding.GetEncoding("x-IA5-Swedish"),
                20108 => Encoding.GetEncoding("x-IA5-Norwegian"),
                20127 => Encoding.GetEncoding("us-ascii"),
                20261 => Encoding.GetEncoding("x-cp20261"),
                20269 => Encoding.GetEncoding("x-cp20269"),
                20273 => Encoding.GetEncoding("IBM273"),
                20277 => Encoding.GetEncoding("IBM277"),
                20278 => Encoding.GetEncoding("IBM278"),
                20280 => Encoding.GetEncoding("IBM280"),
                20284 => Encoding.GetEncoding("IBM284"),
                20285 => Encoding.GetEncoding("IBM285"),
                20290 => Encoding.GetEncoding("IBM290"),
                20297 => Encoding.GetEncoding("IBM297"),
                20420 => Encoding.GetEncoding("IBM420"),
                20423 => Encoding.GetEncoding("IBM423"),
                20424 => Encoding.GetEncoding("IBM424"),
                20833 => Encoding.GetEncoding("x-EBCDIC-KoreanExtended"),
                20838 => Encoding.GetEncoding("IBM-Thai"),
                20866 => Encoding.GetEncoding("koi8-r"),
                20871 => Encoding.GetEncoding("IBM871"),
                20880 => Encoding.GetEncoding("IBM880"),
                20905 => Encoding.GetEncoding("IBM905"),
                20924 => Encoding.GetEncoding("IBM00924"),
                20932 => Encoding.GetEncoding("EUC-JP"),
                20936 => Encoding.GetEncoding("x-cp20936"),
                20949 => Encoding.GetEncoding("x-cp20949"),
                21025 => Encoding.GetEncoding("cp1025"),
                21027 => Encoding.GetEncoding(string.Empty),
                21866 => Encoding.GetEncoding("koi8-u"),
                28591 => Encoding.GetEncoding("iso-8859-1"),
                28592 => Encoding.GetEncoding("iso-8859-2"),
                28593 => Encoding.GetEncoding("iso-8859-3"),
                28594 => Encoding.GetEncoding("iso-8859-4"),
                28595 => Encoding.GetEncoding("iso-8859-5"),
                28596 => Encoding.GetEncoding("iso-8859-6"),
                28597 => Encoding.GetEncoding("iso-8859-7"),
                28598 => Encoding.GetEncoding("iso-8859-8"),
                28599 => Encoding.GetEncoding("iso-8859-9"),
                28603 => Encoding.GetEncoding("iso-8859-13"),
                28605 => Encoding.GetEncoding("iso-8859-15"),
                29001 => Encoding.GetEncoding("x-Europa"),
                32768 => Encoding.GetEncoding("macintosh"),
                32769 => Encoding.GetEncoding("windows-1252"),
                38598 => Encoding.GetEncoding("iso-8859-8-i"),
                50220 => Encoding.GetEncoding("iso-2022-jp"),
                50221 => Encoding.GetEncoding("csISO2022JP"),
                50222 => Encoding.GetEncoding("iso-2022-jp"),
                50225 => Encoding.GetEncoding("iso-2022-kr"),
                50227 => Encoding.GetEncoding("x-cp50227"),
                50229 => Encoding.GetEncoding(string.Empty),
                50930 => Encoding.GetEncoding(string.Empty),
                50931 => Encoding.GetEncoding(string.Empty),
                50933 => Encoding.GetEncoding(string.Empty),
                50935 => Encoding.GetEncoding(string.Empty),
                50936 => Encoding.GetEncoding(string.Empty),
                50937 => Encoding.GetEncoding(string.Empty),
                50939 => Encoding.GetEncoding(string.Empty),
                51932 => Encoding.GetEncoding("euc-jp"),
                51936 => Encoding.GetEncoding("EUC-CN"),
                51949 => Encoding.GetEncoding("euc-kr"),
                51950 => Encoding.GetEncoding(string.Empty),
                52936 => Encoding.GetEncoding("hz-gb-2312"),
                54936 => Encoding.GetEncoding("GB18030"),
                57002 => Encoding.GetEncoding("x-iscii-de"),
                57003 => Encoding.GetEncoding("x-iscii-be"),
                57004 => Encoding.GetEncoding("x-iscii-ta"),
                57005 => Encoding.GetEncoding("x-iscii-te"),
                57006 => Encoding.GetEncoding("x-iscii-as"),
                57007 => Encoding.GetEncoding("x-iscii-or"),
                57008 => Encoding.GetEncoding("x-iscii-ka"),
                57009 => Encoding.GetEncoding("x-iscii-ma"),
                57010 => Encoding.GetEncoding("x-iscii-gu"),
                57011 => Encoding.GetEncoding("x-iscii-pa"),
                65000 => Encoding.GetEncoding("utf-7"),
                65001 => Encoding.GetEncoding("utf-8"),
                _ => null
            };

            return encoding;
        }
    }
}