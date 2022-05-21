using System.Collections.Generic;
using Clover;

namespace Clover
{
    /// <summary>
    /// [MS-XLS] 2.4.168 MergeCells
    ///  If the count of the merged cells in the document is greater than 1026, the file will contain multiple adjacent MergeCells records.
    /// </summary>
    internal class XlsBiffMergeCells : XlsBiffRecord
    {
        public XlsBiffMergeCells(byte[] bytes)
            : base(bytes)
        {
            ushort count = ReadUInt16(0);

            MergeCells = new List<CellRange>();
            for (int i = 0; i < count; i++)
            {
                short fromRow = ReadInt16(2 + i * 8 + 0);
                short toRow = ReadInt16(2 + i * 8 + 2);
                short fromCol = ReadInt16(2 + i * 8 + 4);
                short toCol = ReadInt16(2 + i * 8 + 6);

                CellRange mergeCell = new CellRange(fromCol, fromRow, toCol, toRow);
                MergeCells.Add(mergeCell);
            }
        }

        public List<CellRange> MergeCells { get; }
    }
}
