﻿using System.Collections.Generic;

namespace Clover
{
    public static class TempList<T>
    {
        static readonly List<T> m_Value = new List<T>();

        public static List<T> Value
        {
            get
            {
                m_Value.Clear();
                return m_Value;
            }
        }
    }

    public static class TempHashSet<T>
    {
        public static HashSet<T> Value = new HashSet<T>();
    }
}