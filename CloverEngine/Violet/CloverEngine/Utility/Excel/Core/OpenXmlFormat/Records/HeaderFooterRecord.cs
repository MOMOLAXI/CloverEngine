using System;
using System.Collections.Generic;
using System.Text;
using Clover;

namespace Clover
{
    internal sealed class HeaderFooterRecord : Record
    {
        public HeaderFooterRecord(HeaderFooter headerFooter) 
        {
            HeaderFooter = headerFooter;
        }

        public HeaderFooter HeaderFooter { get; }
    }
}
