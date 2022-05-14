﻿﻿﻿using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Clover
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    internal struct DefaultValueType
    {
        [FieldOffset(0)]
        public bool ValueBool;

        [FieldOffset(0)]
        public int ValueInt32;

        [FieldOffset(0)]
        public long ValueInt64;

        [FieldOffset(0)]
        public float ValueFloat;

        [FieldOffset(0)]
        public double ValueDouble;

        [FieldOffset(0)]
        public ActorID valueActor;
    }

    [Serializable]
    public struct Var
    {
        object m_RefData;
        DefaultValueType m_ValueData;

        public static Var zero => new Var();

        public bool isZero => type == VarType.None;

        public VarType type { get; set; }

        internal VarFlag flag { get; }

        public bool GetBool()
        {
            if (type != VarType.Bool)
            {
                return false;
            }

            return m_ValueData.ValueBool;
        }

        public bool boolValue
        {
            set
            {
                type = VarType.Bool;
                m_ValueData.ValueBool = value;
            }
        }

        public int GetInt()
        {
            if (type != VarType.Int32)
            {
                return 0;
            }

            return m_ValueData.ValueInt32;
        }

        public int intValue
        {
            set
            {
                type = VarType.Int32;
                m_ValueData.ValueInt32 = value;
            }
        }

        public long GetInt64()
        {
            if (type != VarType.Int64)
            {
                return 0;
            }

            return m_ValueData.ValueInt64;
        }

        public long longValue
        {
            set
            {
                type = VarType.Int64;
                m_ValueData.ValueInt64 = value;
            }
        }

        public float GetFloat()
        {
            if (type != VarType.Float)
            {
                return 0;
            }

            return m_ValueData.ValueFloat;
        }

        public float floatValue
        {
            set
            {
                type = VarType.Float;
                m_ValueData.ValueFloat = value;
            }
        }

        public double GetDouble()
        {
            if (type != VarType.Double)
            {
                return 0;
            }

            return m_ValueData.ValueDouble;
        }

        public double doubleValue
        {
            set
            {
                type = VarType.Double;
                m_ValueData.ValueDouble = value;
            }
        }

        public string GetString()
        {
            if (type != VarType.String)
            {
                return string.Empty;
            }

            if (flag != VarFlag.StringInContainer)
            {
                return m_RefData as string;
            }

            if (!(m_RefData is StringBuffer sc))
            {
                return m_RefData as string;
            }

            int strID = m_ValueData.ValueInt32;
            return sc[strID];
        }

        public string stringValue
        {
            set
            {
                type = VarType.String;
                m_RefData = value;
            }
        }

        public ActorID GetActorID()
        {
            if (type != VarType.Object)
            {
                return ActorID.zero;
            }

            return m_ValueData.valueActor;
        }

        public ActorID actorValue
        {
            set
            {
                type = VarType.Object;
                m_ValueData.valueActor = value;
            }
        }

        public byte[] GetBinary()
        {
            if (type != VarType.Binary)
            {
                return null;
            }

            return m_RefData as byte[];
        }

        public byte[] binaryValue
        {
            set
            {
                type = VarType.Binary;
                m_RefData = value;
            }
        }

        public object GetObject()
        {
            return type != VarType.UserData ? null : m_RefData;
        }

        public object userDataValue
        {
            set
            {
                type = VarType.UserData;
                m_RefData = value;
            }
        }

        public bool StringEquals(Var other)
        {
            if (flag == VarFlag.StringInContainer && other.flag == VarFlag.StringInContainer)
            {
                if (m_RefData != other.m_RefData)
                {
                    return false;
                }

                if (!(m_RefData is StringBuffer sc))
                {
                    return false;
                }

                return sc.StrEquals(m_ValueData.ValueInt32, other.m_ValueData.ValueInt32);
            }

            bool isEmpty1 = IsEmptyString();
            bool isEmpty2 = other.IsEmptyString();

            if (isEmpty1 != isEmpty2)
            {
                return false;
            }

            if (isEmpty1)
            {
                return true;
            }

            return GetString() == other.GetString();
        }

        public Var(bool value)
        {
            type = VarType.Bool;
            flag = VarFlag.None;
            m_RefData = null;
            m_ValueData = new DefaultValueType {ValueBool = value};
        }

        public Var(int value)
        {
            type = VarType.Int32;
            flag = VarFlag.None;
            m_RefData = null;
            m_ValueData = new DefaultValueType {ValueInt32 = value};
        }

        public Var(long value)
        {
            type = VarType.Int64;
            flag = VarFlag.None;
            m_RefData = null;
            m_ValueData = new DefaultValueType {ValueInt64 = value};
        }

        public Var(float value)
        {
            type = VarType.Float;
            flag = VarFlag.None;
            m_RefData = null;
            m_ValueData = new DefaultValueType {ValueFloat = value};
        }

        public Var(double value)
        {
            type = VarType.Double;
            flag = VarFlag.None;
            m_RefData = null;
            m_ValueData = new DefaultValueType {ValueDouble = value};
        }

        public Var(string value)
        {
            type = VarType.String;
            flag = VarFlag.None;
            m_RefData = value ?? string.Empty;
            m_ValueData = new DefaultValueType();
        }

        public Var(ActorID value)
        {
            type = VarType.Object;
            flag = VarFlag.None;
            m_RefData = null;
            m_ValueData = new DefaultValueType {valueActor = value};
        }

        public Var(byte[] value)
        {
            type = VarType.Binary;
            flag = VarFlag.None;
            m_RefData = value;
            m_ValueData = new DefaultValueType();
        }

        public Var(object value)
        {
            type = VarType.UserData;
            flag = VarFlag.None;
            m_RefData = value;
            m_ValueData = new DefaultValueType();
        }

        internal Var(StringBuffer sc, int strID)
        {
            type = VarType.String;
            flag = VarFlag.StringInContainer;
            m_RefData = sc;
            m_ValueData = new DefaultValueType {ValueInt32 = strID};
        }

        public override string ToString()
        {
            switch (type)
            {
                case VarType.Bool:
                    return m_ValueData.ValueBool.ToString();
                case VarType.Int32:
                    return m_ValueData.ValueInt32.ToString();
                case VarType.Int64:
                    return m_ValueData.ValueInt64.ToString();
                case VarType.Float:
                    return m_ValueData.ValueFloat.ToString(CultureInfo.InvariantCulture);
                case VarType.Double:
                    return m_ValueData.ValueDouble.ToString(CultureInfo.InvariantCulture);
                case VarType.String:
                case VarType.WideStr:
                    return GetString();
                case VarType.Object:
                    return m_ValueData.valueActor.ToString();
                case VarType.None:
                    break;
                case VarType.UserData:
                    break;
                case VarType.Binary:
                    break;
                case VarType.Max:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return string.Empty;
        }

        bool IsEmptyString()
        {
            if (type != VarType.String)
            {
                return true;
            }

            if (flag != VarFlag.StringInContainer)
            {
                return GetString().Length <= 0;
            }

            if (!(m_RefData is StringBuffer sc))
            {
                return true;
            }

            return sc.IsEmpty(m_ValueData.ValueInt32);
        }
    }
}