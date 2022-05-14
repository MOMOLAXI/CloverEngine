using System;

namespace Clover
{
    public static partial class Helper
    {
        public static int QueryIntArgs(object[] args, int index, int defaultVal = 0)
        {
            object obj = args.SafeGetData(index);
            return ToInt(obj, defaultVal);
        }

        public static int QueryIntArgs(string[] args, int index, int defaultVal = 0)
        {
            if (index < 0 || null == args || args.Length <= index)
            {
                return defaultVal;
            }

            return Helper.ToInt(args[index]);
        }

        public static long QueryInt64Args(object[] args, int index, long defaultVal = 0)
        {
            if (index < 0 || null == args || args.Length <= index)
            {
                return defaultVal;
            }

            object obj = args[index];
            return ToLong(obj, defaultVal);
        }

        public static bool QueryBoolArgs(object[] args, int index, bool defaultVal = false)
        {
            if (index < 0 || null == args || args.Length <= index)
            {
                return defaultVal;
            }

            object obj = args[index];

            switch (obj)
            {
                case bool b:
                {
                    return b;
                }
                case int v:
                {
                    return v != 0;
                }
                case string s:
                {
                    return s.ParseBool();
                }
                default:
                {
                    float value = ToFloat(obj, 0);

                    return Math.Abs(value) > float.Epsilon || defaultVal;
                }
            }
        }

        public static T QueryStructArgs<T>(object[] args, int index, T defaultVal = default(T)) where T : struct
        {
            if (index < 0 || null == args || args.Length <= index)
            {
                return defaultVal;
            }

            if (args[index] is T)
            {
                return (T) args[index];
            }

            return defaultVal;
        }

        public static bool QueryBoolArgs(string[] args, int index, bool defaultVal = false)
        {
            if (index < 0 || null == args || args.Length <= index)
            {
                return defaultVal;
            }

            return Helper.ToBool(args[index]);
        }

        public static float QueryFloatArgs(object[] args, int index, float defaultVal = 0)
        {
            if (index < 0 || null == args || args.Length <= index)
            {
                return defaultVal;
            }

            object obj = args[index];
            return ToFloat(obj, defaultVal);
        }

        /// <summary>
        /// 查询对象参数
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <param name="defaultVal"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T QueryClassArgs<T>(object[] args, int index, T defaultVal = default) where T : class
        {
            if (args == null)
            {
                return default;
            }

            if (!IsValidIndex(index, args.Length))
            {
                return defaultVal;
            }

            if (args[index] is T t)
            {
                return t;
            }

            return defaultVal;
        }

        public static float ToFloat(this object obj, float defaultVal)
        {
            if (obj == null)
            {
                return defaultVal;
            }

            return obj switch
            {
                float result => float.IsNaN(result) ? defaultVal : result,
                int i => i,
                long l => l,
                string s => s.ParseFloat(),
                char c => c,
                short sh => sh,
                byte b => b,
                sbyte sb => sb,
                uint u => u,
                ulong ul => ul,
                _ => defaultVal
            };
        }

        public static long ToLong(this object obj, long defaultVal)
        {
            if (obj == null)
            {
                return defaultVal;
            }

            return obj switch
            {
                long l => l,
                int i => i,
                string s => s.ParseInt64(),
                char c => c,
                short sh => sh,
                byte b => b,
                sbyte sb => sb,
                uint u => u,
                _ => defaultVal
            };
        }

        public static int ToInt(this object obj, int defaultVal)
        {
            if (obj == null)
            {
                return defaultVal;
            }

            return obj switch
            {
                int i => i,
                Enum _ => (int) obj,
                string s1 => s1.ParseInt(),
                char c => c,
                short s => s,
                byte b => b,
                sbyte @sbyte => @sbyte,
                long l => (int) l,
                _ => defaultVal
            };
        }
    }
}