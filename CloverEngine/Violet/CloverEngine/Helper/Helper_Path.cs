﻿﻿﻿﻿﻿namespace Clover
{
    public static partial class Helper
    {
        public static string Append(string spliter, params string[] args)
        {
            if (string.IsNullOrEmpty(spliter) || args == null)
            {
                return string.Empty;
            }

            string result = string.Empty;
            Foreach(args, (arg, index) =>
            {
                if (index == arg.Length - 1)
                {
                    result += arg;
                }
                else
                {
                    result = result + spliter + arg;
                }
            });

            return result;
        }
    }
}