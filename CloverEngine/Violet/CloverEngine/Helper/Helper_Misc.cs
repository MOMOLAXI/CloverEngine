using System;

namespace Clover
{
    public static partial class Helper
    {
        /// <summary>
        /// Swap Generic
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <typeparam name="T"></typeparam>
        public static void Swap<T>(ref T left, ref T right)
        {
            T temp = left;
            left = right;
            right = temp;
        }

        public static void Swap<T>(this T[] array, int left, int right)
        {
            if (!IsValidIndex(left, array.Length) || !IsValidIndex(right, array.Length))
            {
                return;
            }

            T temp = array[left];
            array[left] = array[right];
            array[right] = temp;
        }

        /// <summary>
        /// Swap
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void PlusSwap(ref int left, ref int right)
        {
            left += right;
            right = left - right;
            left -= right;
        }

        /// <summary>
        /// Swap
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void Swap(ref int left, ref int right)
        {
            left ^= right;
            right ^= left;
            left ^= right;
        }

        /// <summary>
        /// Swap
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void Swap(ref long left, ref long right)
        {
            left ^= right;
            right ^= left;
            left ^= right;
        }

        /// <summary>
        /// Swap
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void Swap(ref ulong left, ref ulong right)
        {
            left ^= right;
            right ^= left;
            left ^= right;
        }

        /// <summary>
        /// 使用频度高的地方慎用！
        /// 转换成枚举，因为当前C#版本没法限定为枚举，暂时限制为值类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ParseEnum<T>(this string value, out T result) where T : Enum
        {
            result = default;
            try
            {
                result = (T) Enum.Parse(typeof(T), value);
                return true;
            }
            catch (Exception e)
            {
                Log.InternalError(e);
                return false;
            }
        }

        public static int ParseInt(this string value)
        {
            int.TryParse(value, out int ret);
            return ret;
        }

        public static uint ParseUInt(this string value)
        {
            uint.TryParse(value, out uint ret);
            return ret;
        }

        public static bool ParseBool(this string value)
        {
            if (ToInt(value) > 0)
            {
                return true;
            }

            bool.TryParse(value, out bool ret);
            return ret;
        }

        public static long ParseInt64(this string value)
        {
            long.TryParse(value, out long ret);
            return ret;
        }

        public static ulong ParseUInt64(this string value)
        {
            ulong.TryParse(value, out ulong ret);
            return ret;
        }

        public static float ParseFloat(this string value)
        {
            float.TryParse(value, out float ret);
            return ret;
        }
    }
}