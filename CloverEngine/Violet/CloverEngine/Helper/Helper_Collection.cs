﻿﻿﻿﻿using System.Collections.Generic;

namespace Clover
{
    public static partial class Helper
    {
        /// <summary>
        /// 异常捕捉循环
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="runner"></param>
        /// <typeparam name="T"></typeparam>
        public static void Foreach<T>(T[] elements, Run<T> runner)
        {
            if (elements == null || runner == null)
            {
                return;
            }

            for (int i = 0; i < elements.Length; ++i)
            {
                T element = elements[i];
                Function.Run(runner, element);
            }
        }

        /// <summary>
        /// 异常捕捉循环
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="runner"></param>
        /// <typeparam name="T"></typeparam>
        public static void Foreach<T>(T[] elements, Run<T, int> runner)
        {
            if (elements == null || runner == null)
            {
                return;
            }

            for (int i = 0; i < elements.Length; ++i)
            {
                T element = elements[i];
                Function.Run(runner, element, i);
            }
        }

        /// <summary>
        /// 异常捕捉循环
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="runner"></param>
        /// <typeparam name="T"></typeparam>
        public static void Foreach<T>(IList<T> elements, Run<T> runner)
        {
            if (elements == null || runner == null)
            {
                return;
            }

            for (int i = 0; i < elements.Count; ++i)
            {
                T element = elements[i];
                Function.Run(runner, element);
            }
        }

        /// <summary>
        /// 异常捕捉循环
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="runner"></param>
        /// <typeparam name="T"></typeparam>
        public static void Foreach<T>(IList<T> elements, Run<T, int> runner)
        {
            if (elements == null || runner == null)
            {
                return;
            }

            for (int i = 0; i < elements.Count; ++i)
            {
                T element = elements[i];
                Function.Run(runner, element, i);
            }
        }

        /// <summary>
        /// 异常捕捉循环
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="runner"></param>
        /// <typeparam name="T"></typeparam>
        public static void Foreach<T>(HashSet<T> elements, Run<T> runner)
        {
            if (elements == null || runner == null)
            {
                return;
            }

            foreach (T element in elements)
            {
                Function.Run(runner, element);
            }
        }

        /// <summary>
        /// 异常捕捉循环
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="runner"></param>
        public static void Foreach<K, V>(IDictionary<K, V> elements, Run<K, V> runner)
        {
            if (elements == null || runner == null)
            {
                return;
            }

            foreach (KeyValuePair<K, V> element in elements)
            {
                Function.Run(runner, element.Key, element.Value);
            }
        }

        public static bool IsValidIndex(int index, int count)
        {
            return index > -1 && index < count;
        }
    }
}