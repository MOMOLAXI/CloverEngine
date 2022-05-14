﻿using System.Collections.Generic;

namespace Clover
{
    public class PropertyCollection
    {
        static readonly ObjectPool<CloverActorProperty> m_Pool = new ObjectPool<CloverActorProperty>();

        public readonly Dictionary<string, CloverActorProperty> Properties =
            new Dictionary<string, CloverActorProperty>();

        /// <summary>
        /// 所属类
        /// </summary>
        public string ActorName { get; set; }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public static PropertyCollection Create()
        {
            PropertyCollection collection = new PropertyCollection();
            collection.Reset();
            return collection;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="propName"></param>
        public CloverActorProperty Add(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError($"property name is null or empty while add property");
                return Empty<CloverActorProperty>.Value;
            }

            CloverActorProperty property = m_Pool.Get();
            property.Name = propName;
            Properties.Add(property.Name, property);
            return property;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public CloverActorProperty Get(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                return Empty<CloverActorProperty>.Value;
            }

            if (Properties.TryGetValue(propName, out CloverActorProperty property))
            {
                return property;
            }

            property = Add(propName);
            return property;
        }

        public void Reset()
        {
            foreach (KeyValuePair<string, CloverActorProperty> property in Properties)
            {
                CloverActorProperty prop = property.Value;
                prop.Reset();
                m_Pool.Release(prop);
            }

            Properties.Clear();
        }
    }
}