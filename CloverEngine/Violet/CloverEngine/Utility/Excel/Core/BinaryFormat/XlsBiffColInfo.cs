using System;
using Clover;

namespace Clover
{
    internal class XlsBiffColInfo : XlsBiffRecord
    {
        public XlsBiffColInfo(byte[] bytes)
            : base(bytes)
        {
            ushort colFirst = ReadUInt16(0x0);
            ushort colLast = ReadUInt16(0x2);
            ushort colDx = ReadUInt16(0x4);
            ColInfoSettings flags = (ColInfoSettings)ReadUInt16(0x8);
            bool userSet = (flags & ColInfoSettings.UserSet) != 0;
            bool hidden = (flags & ColInfoSettings.Hidden) != 0;

            Value = new Column(colFirst, colLast, hidden, userSet ? (double?)colDx / 256.0 : null);
        }

        [Flags]
        private enum ColInfoSettings
        {
            Hidden = 0b01,
            UserSet = 0b10,
        }

        public Column Value { get; }
    }
}
