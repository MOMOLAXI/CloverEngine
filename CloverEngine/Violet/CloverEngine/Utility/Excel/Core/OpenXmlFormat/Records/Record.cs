using System;
using System.Collections.Generic;
using System.Text;

namespace Clover
{
    internal abstract class Record
    {
        internal static Record Default { get; } = new DefaultRecord();

        private sealed class DefaultRecord : Record
        {
        }
    }
}
