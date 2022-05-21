using System;
using System.Collections.Generic;
using System.Text;
using Clover;

namespace Clover
{
    internal sealed class CellStyleExtendedFormatRecord : Record
    {
        public CellStyleExtendedFormatRecord(ExtendedFormat extendedFormat)
        {
            ExtendedFormat = extendedFormat;
        }

        public ExtendedFormat ExtendedFormat { get; }
    }
}
