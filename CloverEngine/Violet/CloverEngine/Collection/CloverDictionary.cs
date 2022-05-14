﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clover
{
    /// <summary>
    /// 序列化字典
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    [Serializable]
    public class CloverDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
    {
        /// <summary>
        /// 序列化Keys
        /// </summary>
        [SerializeField]
        public List<K> keys = new List<K>();

        /// <summary>
        /// 序列化Values
        /// </summary>
        [SerializeField]
        public List<V> values = new List<V>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach (KeyValuePair<K, V> kvp in this)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                K key = keys[i];
                V value = values[i];
                if (key != null && value != null)
                {
                    this[key] = value;
                }
            }
        }
    }
}