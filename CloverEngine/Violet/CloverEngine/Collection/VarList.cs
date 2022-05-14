﻿﻿﻿using System.Collections.Generic;
using System.Text;

namespace Clover
{
    public class VarList : IVarList
    {
        readonly List<Var> m_Values = new List<Var>();

        static readonly List<VarList> m_UsingList = new List<VarList>(100);
        static readonly List<VarList> m_FreeList = new List<VarList>(100);
        static readonly StringBuilder m_SB = new StringBuilder();

        public static VarList Empty { get; } = new VarList();

        public int Count => m_Values.Count;

        /// <summary>
        /// 分配一个自动释放的VarList，会在本帧末尾释放，不能持有
        /// </summary>
        /// <returns></returns>
        public static VarList AllocAutoVarList()
        {
            if (m_FreeList.Count == 0)
            {
                VarList varList = new VarList();
                m_UsingList.Add(varList);

                return varList;
            }
            else
            {
                int pos = m_FreeList.Count - 1;

                VarList varList = m_FreeList[pos];
                m_FreeList.RemoveAt(pos);
                m_UsingList.Add(varList);

                return varList;
            }
        }

        public static void AutoRelease()
        {
            if (m_UsingList.Count == 0)
            {
                return;
            }

            for (int i = 0; i < m_UsingList.Count; ++i)
            {
                VarList varList = m_UsingList[i];
                if (varList.Count > 20)
                {
                    // 参数过多的VarList不回收
                    continue;
                }

                if (m_FreeList.Count >= 20)
                {
                    // 保持缓存的最大数量
                    break;
                }

                varList.Clear();
                m_FreeList.Add(varList);
            }

            m_UsingList.Clear();
        }

        public IVarList Clone()
        {
            VarList result = new VarList();
            result.AddVarList(this);
            return result;
        }

        public void Clear()
        {
            m_Values.Clear();
        }

        public bool Insert(int index, Var value)
        {
            if (index < 0 || index > m_Values.Count)
            {
                return false;
            }

            m_Values.Insert(index, value);

            return true;
        }

        public void RemoveAt(int index)
        {
            m_Values.RemoveAt(index);
        }

        public void AddVar(Var value)
        {
            m_Values.Add(value);
        }

        public void AddBool(bool value)
        {
            m_Values.Add(new Var(value));
        }

        public void AddInt(int value)
        {
            m_Values.Add(new Var(value));
        }

        public void AddInt64(long value)
        {
            m_Values.Add(new Var(value));
        }

        public void AddFloat(float value)
        {
            m_Values.Add(new Var(value));
        }

        public void AddDouble(double value)
        {
            m_Values.Add(new Var(value));
        }

        public void AddString(string value)
        {
            if (value == null)
            {
                Log.InternalError("[VarList:AddBinary] string is null.");
            }

            m_Values.Add(new Var(value));
        }

        public void AddObject(ActorID value)
        {
            m_Values.Add(new Var(value));
        }

        public void AddBinary(byte[] value)
        {
            if (value == null)
            {
                Log.InternalError("[VarList:AddBinary] binary is null.");
            }

            m_Values.Add(new Var(value));
        }

        public void AddVarList(IVarList varList)
        {
            if (varList == null)
            {
                return;
            }

            if (this == varList)
            {
                Log.InternalError("[VarList:AddVarList] VarList want to add Owner, please check it.");
            }

            int count = varList.Count;
            for (int i = 0; i < count; ++i)
            {
                m_Values.Add(varList.GetVar(i));
            }
        }

        public VarType GetVarType(int index)
        {
            if (index < 0 || index >= m_Values.Count)
            {
                return VarType.None;
            }

            return m_Values[index].type;
        }

        public Var GetVar(int index)
        {
            if (index < 0 || index >= m_Values.Count)
            {
                return Var.zero;
            }

            return m_Values[index];
        }

        public bool GetBool(int index)
        {
            if (index < 0 || index >= m_Values.Count)
            {
                return false;
            }

            return m_Values[index].GetBool();
        }

        public int GetInt32(int index)
        {
            if (index < 0 || index >= m_Values.Count)
            {
                return 0;
            }

            return m_Values[index].GetInt();
        }

        public long GetInt64(int index)
        {
            if (index < 0 || index >= m_Values.Count)
            {
                return 0;
            }

            return m_Values[index].GetInt64();
        }

        public float GetFloat(int index)
        {
            if (index < 0 || index >= m_Values.Count)
            {
                return 0;
            }

            return m_Values[index].GetFloat();
        }

        public double GetDouble(int index)
        {
            if (index < 0 || index >= m_Values.Count)
            {
                return 0;
            }

            return m_Values[index].GetDouble();
        }

        public string GetString(int index)
        {
            if (index < 0 || index >= m_Values.Count)
            {
                return string.Empty;
            }

            return m_Values[index].GetString();
        }

        public ActorID GetObject(int index)
        {
            if (index < 0 || index >= m_Values.Count)
            {
                return new ActorID();
            }

            return m_Values[index].GetActorID();
        }

        public byte[] GetBinary(int index)
        {
            if (index < 0 || index >= m_Values.Count)
            {
                return null;
            }

            return m_Values[index].GetBinary();
        }

        public static VarList operator <(VarList varList, Var value)
        {
            varList.AddVar(value);
            return varList;
        }

        public static VarList operator <(VarList varList, bool value)
        {
            varList.AddBool(value);
            return varList;
        }

        public static VarList operator <(VarList varList, int value)
        {
            varList.AddInt(value);
            return varList;
        }

        public static VarList operator <(VarList varList, long value)
        {
            varList.AddInt64(value);
            return varList;
        }

        public static VarList operator <(VarList varList, float value)
        {
            varList.AddFloat(value);
            return varList;
        }

        public static VarList operator <(VarList varList, double value)
        {
            varList.AddDouble(value);
            return varList;
        }

        public static VarList operator <(VarList varList, string value)
        {
            varList.AddString(value);
            return varList;
        }

        public static VarList operator <(VarList varList, ActorID value)
        {
            varList.AddObject(value);
            return varList;
        }

        public static VarList operator <(VarList varList, byte[] value)
        {
            varList.AddBinary(value);
            return varList;
        }

        public static VarList operator <(VarList varList, IVarList value)
        {
            varList.AddVarList(value);
            return varList;
        }

        public static VarList operator >(VarList varList, Var value)
        {
            varList.AddVar(value);
            return varList;
        }

        public static VarList operator >(VarList varList, bool value)
        {
            varList.AddBool(value);
            return varList;
        }

        public static VarList operator >(VarList varList, int value)
        {
            varList.AddInt(value);
            return varList;
        }

        public static VarList operator >(VarList varList, long value)
        {
            varList.AddInt64(value);
            return varList;
        }

        public static VarList operator >(VarList varList, float value)
        {
            varList.AddFloat(value);
            return varList;
        }

        public static VarList operator >(VarList varList, double value)
        {
            varList.AddDouble(value);
            return varList;
        }

        public static VarList operator >(VarList varList, string value)
        {
            varList.AddString(value);
            return varList;
        }

        public static VarList operator >(VarList varList, ActorID value)
        {
            varList.AddObject(value);
            return varList;
        }

        public static VarList operator >(VarList varList, byte[] value)
        {
            varList.AddBinary(value);
            return varList;
        }

        public static VarList operator >(VarList varList, IVarList value)
        {
            varList.AddVarList(value);
            return varList;
        }

        public override string ToString()
        {
            m_SB.Length = 0;

            for (int i = 0; i < m_Values.Count; ++i)
            {
                m_SB.Append(m_Values[i].ToString());
                m_SB.Append(";");
            }

            return m_SB.ToString();
        }
    }
}