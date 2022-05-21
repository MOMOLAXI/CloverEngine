using System;
using System.Collections.Generic;
using System.Text;
using Clover;

namespace Clover
{
    internal sealed class MergeCellRecord : Record
    {
        public MergeCellRecord(CellRange range) 
        {
            Range = range;
        }

        public CellRange Range { get; }
    }
}
