﻿using System.Collections.Generic;

namespace Clover
{
    internal static class ActorIDHelper
    {
        static readonly Dictionary<EResType, uint> m_Serial = new Dictionary<EResType, uint>();

        public static ActorID Generate(EResType resType)
        {
            if (m_Serial.TryGetValue(resType, out uint serial))
            {
                uint newSerial = ++serial;
                m_Serial[resType] = newSerial;
                ActorID id = new ActorID((uint) resType, newSerial);
                return id;
            }

            m_Serial[resType] = 0;
            return new ActorID((uint) resType, 0);
        }
    }
}