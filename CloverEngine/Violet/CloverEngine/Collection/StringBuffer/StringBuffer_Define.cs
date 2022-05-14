﻿﻿﻿namespace Clover
{
    /// <summary>
    /// 字符串容器
    /// </summary>
    internal partial class StringBuffer
    {
        struct StringData
        {
            public int pos;
            public int maxCount;
            public bool used;

            public EncodingType encoding;
            public string str;
            public int count;
        }

        public enum EncodingType
        {
            Default,
            UTF8,
            Unicode,
        }
    }
}