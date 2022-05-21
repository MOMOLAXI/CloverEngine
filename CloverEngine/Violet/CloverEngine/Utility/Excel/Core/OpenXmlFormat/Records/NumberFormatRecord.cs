using System;
using System.Collections.Generic;
using System.Text;

namespace Clover
{
    internal sealed class NumberFormatRecord : Record
    {
        public NumberFormatRecord(int formatIndexInFile, string formatString) 
        {
            FormatIndexInFile = formatIndexInFile;
            FormatString = formatString;
        }

        public int FormatIndexInFile { get; }

        public string FormatString { get; }
    }
}
