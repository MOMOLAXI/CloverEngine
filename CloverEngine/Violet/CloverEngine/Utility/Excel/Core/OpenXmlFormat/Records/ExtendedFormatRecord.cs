using System;
using System.Collections.Generic;
using System.Text;
using Clover;

namespace Clover
{
    internal sealed class ExtendedFormatRecord : Record
    {
        public ExtendedFormatRecord(ExtendedFormat extendedFormat) 
        {
            ExtendedFormat = extendedFormat;
        }

        public ExtendedFormat ExtendedFormat { get; }
    }
}
