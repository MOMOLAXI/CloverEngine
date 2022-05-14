﻿﻿using System;
using System.Text;

namespace Clover
{
    /// <summary>
    /// 字符串容器
    /// </summary>
    internal partial class StringBuffer
    {
        byte[] m_Buffer;
        int m_Pos;

        readonly RapidList<StringData> m_DataList = new RapidList<StringData>();
        int m_UsedCount;


        public const int INVALID_ID = -1;

        public int capacity => m_Buffer.Length;

        public string this[int id]
        {
            get
            {
                if (id < 0 || id >= m_DataList.Count)
                {
                    return string.Empty;
                }

                StringData data = m_DataList[id];
                if (!data.used)
                {
                    return string.Empty;
                }

                if (data.str == null)
                {
                    switch (data.encoding)
                    {
                        case EncodingType.Default:
                            data.str = Encoding.UTF8.GetString(m_Buffer, data.pos, data.count);
                            break;
                        case EncodingType.UTF8:
                            data.str = Encoding.UTF8.GetString(m_Buffer, data.pos, data.count);
                            break;
                        case EncodingType.Unicode:
                            data.str = Encoding.Unicode.GetString(m_Buffer, data.pos, data.count);
                            break;
                        default:
                            data.str = string.Empty;
                            // global::Log.InternalError("[StringBuffer:this[]]invalid encoding type " + (int) data.encoding);
                            break;
                    }

                    m_DataList[id] = data;
                }

                return data.str;
            }
        }

        public StringBuffer(int capacity = DigitConst.N_2048)
        {
            capacity = Math.Max(capacity, DigitConst.N_256);
            m_Buffer = new byte[capacity];
        }

        public int Create(byte[] data, int startIndex, int count)
        {
            return Create(EncodingType.Default, data, startIndex, count);
        }

        public int Create(EncodingType encoding, byte[] data, int startIndex, int count)
        {
            if (startIndex < 0 || count <= 0 || startIndex + count >= data.Length)
            {
                return INVALID_ID;
            }

            int index = AllocNew(count);
            if (index == INVALID_ID)
            {
                return INVALID_ID;
            }

            StringData stringData = m_DataList[index];
            Array.Copy(data, startIndex, m_Buffer, stringData.pos, count);
            stringData.used = true;
            stringData.count = count;
            stringData.encoding = encoding;
            m_DataList[index] = stringData;

            ++m_UsedCount;

            return index;
        }

        public void Release(int id)
        {
            if (id < 0 || id >= m_DataList.Count)
            {
                return;
            }

            StringData data = m_DataList[id];
            if (data.used)
            {
                --m_UsedCount;

                data.used = false;
                data.encoding = EncodingType.Default;
                data.str = null;

                m_DataList[id] = data;
            }
        }

        public void Clear()
        {
            m_Pos = 0;
            m_DataList.Clear();
            m_UsedCount = 0;
        }

        public bool IsEmpty(int id)
        {
            if (id < 0 || id >= m_DataList.Count)
            {
                return true;
            }

            StringData data = m_DataList[id];
            if (!data.used)
            {
                return true;
            }

            if (data.count <= 0)
            {
                return true;
            }

            return false;
        }

        public bool StrEquals(int id1, int id2)
        {
            if (id1 < 0 || id1 >= m_DataList.Count ||
                id2 < 0 || id2 >= m_DataList.Count)
            {
                return false;
            }

            if (id1 == id2)
            {
                return true;
            }

            StringData data1 = m_DataList[id1];
            StringData data2 = m_DataList[id2];

            if (!data1.used && !data2.used)
            {
                return true;
            }

            if (data1.count != data2.count)
            {
                return false;
            }

            for (int i = 0; i < data1.count; ++i)
            {
                if (m_Buffer[data1.pos + i] != m_Buffer[data2.pos + i])
                {
                    return false;
                }
            }

            return true;
        }

        int AllocNew(int newCount)
        {
            // 缓存不足，拓展
            if (m_Pos + newCount >= m_Buffer.Length)
            {
                Expand(m_Pos + newCount);
            }

            StringData data = new StringData {pos = m_Pos, maxCount = newCount};

            m_Pos += newCount;
            m_DataList.Add(data);

            return m_DataList.Count - 1;
        }

        void Expand(int newLength)
        {
            // 修剪长度到2的幂次方
            newLength = TrimLength(newLength);

            if (newLength <= m_Buffer.Length)
            {
                return;
            }

            byte[] newBuffer = new byte[newLength];
            Array.Copy(m_Buffer, newBuffer, m_Buffer.Length);

            m_Buffer = newBuffer;
        }

        // 修剪长度到2的幂次方
        int TrimLength(int length)
        {
            int count = 1;
            int times = 0;

            while (times++ < 20)
            {
                count *= 2;
                if (count >= length)
                {
                    return count;
                }
            }

            return length;
        }
    }
}