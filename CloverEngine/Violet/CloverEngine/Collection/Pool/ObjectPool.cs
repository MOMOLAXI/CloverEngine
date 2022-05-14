﻿using System.Collections.Generic;

namespace Clover
{
    public class ObjectPool<T> where T : class, ICacheAble, new()
    {
        readonly Stack<T> m_Stack = new Stack<T>();

        /// <summary>
        /// 当前缓存数量
        /// </summary>
        public int CachedCount => m_Stack.Count;

        /// <summary>
        /// 最大缓存数量
        /// </summary>
        public int MaxCacheCount { get; }

        public ObjectPool(int maxCacheCount = DigitConst.N_1024)
        {
            MaxCacheCount = maxCacheCount;
        }

        public T Get()
        {
            T element = m_Stack.Count == 0 ? new T() : m_Stack.Pop();
            element.Reset();
            element.cacheFlag = false;
            return element;
        }

        // 释放进入缓存
        public void Release(T t)
        {
            if (t == null)
            {
                return;
            }

            if (m_Stack.Count >= MaxCacheCount)
            {
                return;
            }

            t.cacheFlag = true;
            t.Reset();
            m_Stack.Push(t);
        }
    }
}