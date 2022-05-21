using System;
using System.Collections.Generic;
using System.Text;

namespace Clover
{
    internal sealed class SheetFormatPrRecord : Record
    {
        public SheetFormatPrRecord(double? defaultRowHeight)
        {
            DefaultRowHeight = defaultRowHeight;
        }

        public double? DefaultRowHeight { get; }
    }
}
