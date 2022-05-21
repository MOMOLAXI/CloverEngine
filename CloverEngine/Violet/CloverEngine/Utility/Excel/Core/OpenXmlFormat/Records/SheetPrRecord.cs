using System;
using System.Collections.Generic;
using System.Text;

namespace Clover
{
    internal sealed class SheetPrRecord : Record
    {
        public SheetPrRecord(string codeName)
        {
            CodeName = codeName;
        }

        public string CodeName { get; }
    }
}
