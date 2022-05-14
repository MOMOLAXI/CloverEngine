﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;

namespace Clover
{
    public class RapidList<T>
    {
        public delegate int CompareFunc(T left, T right);

        // 大量访问容器时，Mono的效率问题导致访问函数存在开销，故而此处直接暴露数据
        public T[] Buffer = EmptyArray<T>.Value;

        public int Count { get; private set; }

        public int capacity
        {
            get => Buffer?.Length ?? 0;

            set
            {
                if (value <= capacity)
                {
                    return;
                }

                int newCapacity = Math.Max(capacity, 32);

                for (int i = 0; i < 32; ++i)
                {
                    if (newCapacity < value)
                    {
                        newCapacity <<= 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (newCapacity <= 0)
                {
                    return;
                }

                T[] newList = new T[newCapacity];
                if (Buffer != null && Count > 0)
                {
                    Buffer.CopyTo(newList, 0);
                }

                Buffer = newList;
            }
        }

        public T this[int i]
        {
            get => Buffer[i];
            set => Buffer[i] = value;
        }

        public void Clear()
        {
            Count = 0;
        }

        public void Add(T item)
        {
            if (Buffer == null || Count == Buffer.Length)
            {
                AllocateMore();
            }

            Buffer[Count++] = item;
        }

        public void Insert(int index, T item)
        {
            if (Buffer == null || Count == Buffer.Length)
            {
                AllocateMore();
            }

            if (index > -1 && index < Count)
            {
                Array.Copy(Buffer, index, Buffer, index + 1, Count - index);

                Buffer[index] = item;
                ++Count;
            }
            else
            {
                Add(item);
            }
        }

        public bool Contains(T item)
        {
            if (Buffer == null)
            {
                return false;
            }

            for (int i = 0; i < Count; ++i)
            {
                if (Buffer[i].Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        public int IndexOf(T item)
        {
            if (Buffer == null)
            {
                return -1;
            }

            for (int i = 0; i < Count; ++i)
            {
                if (Buffer[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Remove(T item)
        {
            if (Buffer != null)
            {
                EqualityComparer<T> comp = EqualityComparer<T>.Default;

                for (int i = 0; i < Count; ++i)
                {
                    if (comp.Equals(Buffer[i], item))
                    {
                        --Count;
                        Buffer[i] = default(T);
                        for (int b = i; b < Count; ++b) Buffer[b] = Buffer[b + 1];
                        Buffer[Count] = default(T);
                        return true;
                    }
                }
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (Buffer != null && index > -1 && index < Count)
            {
                --Count;

                if (Count > index)
                {
                    Array.Copy(Buffer, index + 1, Buffer, index, Count - index);
                }
            }
        }

        public T Pop()
        {
            if (Buffer != null && Count != 0)
            {
                return Buffer[--Count];
            }

            return default(T);
        }

        public T[] ToArray()
        {
            T[] newList = new T[Count];
            for (int i = 0; i < Count; ++i)
            {
                newList[i] = Buffer[i];
            }

            return newList;
        }

        public void Sort(CompareFunc comparer)
        {
            int start = 0;
            int max = Count - 1;
            bool changed = true;

            while (changed)
            {
                changed = false;

                for (int i = start; i < max; ++i)
                {
                    // Compare the two values
                    if (comparer(Buffer[i], Buffer[i + 1]) > 0)
                    {
                        // Swap the values
                        T temp = Buffer[i];
                        Buffer[i] = Buffer[i + 1];
                        Buffer[i + 1] = temp;
                        changed = true;
                    }
                    else if (!changed)
                    {
                        // Nothing has changed -- we can start here next time
                        start = (i == 0) ? 0 : i - 1;
                    }
                }
            }
        }

        void AllocateMore()
        {
            T[] newList = (Buffer != null) ? new T[Math.Max(Buffer.Length << 1, 32)] : new T[32];

            if (Buffer != null && Count > 0)
            {
                Buffer.CopyTo(newList, 0);
            }

            Buffer = newList;
        }
    }
}