﻿﻿﻿﻿using System.Runtime.InteropServices;
using System;

namespace Clover
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct ActorID : IComparable<ActorID>, IEquatable<ActorID>
    {
        [FieldOffset(0)]
        readonly uint m_Ident;

        [FieldOffset(4)]
        readonly uint m_Serial;

        public static ActorID zero;
        public bool IsZero => m_Ident == 0 && m_Serial == 0;
        public uint Ident => m_Ident;
        public uint Serial => m_Serial;
        [field: FieldOffset(0)] public long Value { get; }

        public ActorID(uint ident, uint serial)
        {
            Value = 0;
            m_Ident = ident;
            m_Serial = serial;
        }

        public ActorID(long value)
        {
            m_Ident = 0;
            m_Serial = 0;
            Value = value;
        }

        public ActorID(string id)
        {
            Value = 0;
            m_Ident = 0;
            m_Serial = 0;

            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            ParseID(id, out m_Ident, out m_Serial);
        }

        public static implicit operator long(ActorID obj)
        {
            return obj.Value;
        }

        public static bool operator ==(ActorID a, ActorID b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(ActorID a, ActorID b)
        {
            return a.Value != b.Value;
        }

        public override string ToString()
        {
            return string.Format("[{0}-{1}]", m_Ident, m_Serial);
        }

        public override bool Equals(object obj)
        {
            return Value == ((ActorID) obj).Value;
        }

        public override int GetHashCode()
        {
            return (int) m_Serial;
        }

        int IComparable<ActorID>.CompareTo(ActorID other)
        {
            return Value.CompareTo(other.Value);
        }

        bool IEquatable<ActorID>.Equals(ActorID other)
        {
            return Value == other.Value;
        }

        static void ParseID(string actorID, out uint ident, out uint serial)
        {
            ident = 0;
            serial = 0;
            string[] param = actorID.Split(Default.IDSpliter);
            if (param.Length < 3)
            {
                return;
            }

            uint.TryParse(param[1], out ident);
            uint.TryParse(param[2], out serial);
        }
    }
}