﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;

namespace Clover
{
    /// <summary>
    /// 多态对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PolyObjectPool<T> where T : class, ICacheAble
    {
        readonly Dictionary<Type, Stack<T>> m_TypeMapping = new Dictionary<Type, Stack<T>>();

        public int CachedCount { get; private set; }

        public int CaxCacheCount { get; }

        public PolyObjectPool(int caxCacheCount = DigitConst.N_1024)
        {
            CaxCacheCount = caxCacheCount;
        }

        public E Get<E>() where E : class, T, new()
        {
            if (!m_TypeMapping.TryGetValue(typeof(E), out Stack<T> stack))
            {
                return new E();
            }

            if (stack.Count <= 0)
            {
                return new E();
            }

            --CachedCount;

            T element = stack.Pop();
            if (element is ICacheAble cacheAble)
            {
                cacheAble.cacheFlag = false;
                cacheAble.Reset();
            }

            return element as E;
        }

        public T Get(Type type)
        {
            if (m_TypeMapping.TryGetValue(type, out Stack<T> stack) && stack.Count > 0)
            {
                --CachedCount;

                T element = stack.Pop();
                if (element is ICacheAble cacheAble)
                {
                    cacheAble.cacheFlag = false;
                }

                return element;
            }

            T t = type.Assembly.CreateInstance(type.Name) as T;

            return t;
        }

        public void Release<TP>(TP tp) where TP : T
        {
            if (tp == null)
            {
                return;
            }

            // 只缓存继承了ICacheAble接口的元素
            if (!(tp is ICacheAble t) || t.cacheFlag)
            {
                return;
            }

            Type type = t.GetType();

            if (!m_TypeMapping.TryGetValue(type, out Stack<T> stack))
            {
                stack = new Stack<T>();
                m_TypeMapping.Add(type, stack);
            }

            if (CachedCount >= CaxCacheCount)
            {
                t.cacheFlag = true;
                return;
            }

            ++CachedCount;

            t.cacheFlag = true;
            t.Reset();
            stack.Push(tp);
        }
    }
}