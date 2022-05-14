﻿﻿﻿using System.Collections.Generic;

namespace Clover
{
    public class FastCollection<K, V>
    {
        readonly Dictionary<K, int> m_Query = new Dictionary<K, int>();

        public int Count => All.Count;

        public List<V> All = new List<V>();

        public void Add(K key, V value)
        {
            All.Add(value);
            m_Query[key] = Count - 1;
        }

        public bool TryGet(K key, out V value)
        {
            value = default;
            if (m_Query.TryGetValue(key, out int index))
            {
                value = All[index];
                return true;
            }

            return false;
        }

        public bool Contains(K key)
        {
            return m_Query.ContainsKey(key);
        }

        public void Remove(K key)
        {
            if (m_Query.TryGetValue(key, out int index))
            {
                All.RemoveAt(index);
            }
        }

        public void Clear()
        {
            All.Clear();
            m_Query.Clear();
        }
    }
}