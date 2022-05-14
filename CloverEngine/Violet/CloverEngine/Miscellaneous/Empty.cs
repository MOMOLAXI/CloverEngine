﻿﻿﻿using System.Collections.Generic;

namespace Clover
{
    public static partial class Empty<T> where T : new()
    {
        public static T Value = new T();
    }

    public class EmptyArray<T>
    {
        public static T[] Value = { };
    }

    public class EmptyList<T>
    {
        public static List<T> Value = new List<T>();
    }

    public class EmptyDictionary<K, V>
    {
        public static Dictionary<K, V> Value = new Dictionary<K, V>();
    }

    public class EmptyStack<T>
    {
        public static Stack<T> Value = new Stack<T>();
    }
}