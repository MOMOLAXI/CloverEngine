using System;
using System.Collections.Generic;
using System.Text;
using Clover;

namespace Clover
{
    internal sealed class ColumnRecord : Record
    {
        public ColumnRecord(Column column)
        {
            Column = column;
        }

        public Column Column { get; }
    }
}
