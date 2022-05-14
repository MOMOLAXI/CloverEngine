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
    }
}