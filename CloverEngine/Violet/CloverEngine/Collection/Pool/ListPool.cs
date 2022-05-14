﻿﻿﻿﻿﻿using System.Collections.Generic;

namespace Clover
{
    /// <summary>
    /// 列表缓存池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListPool<T>
    {
        private static Stack<List<T>> m_CacheStack;

        /// <summary>
        /// 申请
        /// </summary>
        /// <returns></returns>
        public static List<T> Allocate()
        {
            List<T> result;
            if (m_CacheStack == null || m_CacheStack.Count == 0)
            {
                result = new List<T>();
            }
            else
            {
                result = m_CacheStack.Pop();
            }

            return result;
        }

        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="t"></param>
        public static void Recycle(List<T> t)
        {
            if (t == null)
            {
                return;
            }

            t.Clear();

            m_CacheStack ??= new Stack<List<T>>();
            m_CacheStack.Push(t);
        }
    }
}