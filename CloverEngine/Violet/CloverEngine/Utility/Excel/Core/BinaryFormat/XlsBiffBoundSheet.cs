using System;
using System.Text;

namespace Clover
{
    /// <summary>
    /// Represents Sheet record in Workbook Globals
    /// </summary>
    internal class XlsBiffBoundSheet : XlsBiffRecord
    {
        readonly IXlsString m_SheetName;

        internal XlsBiffBoundSheet(byte[] bytes, int biffVersion)
            : base(bytes)
        {
            StartOffset = ReadUInt32(0x0);
            Type = (SheetType) ReadByte(0x5);
            VisibleState = (SheetVisibility) ReadByte(0x4);

            m_SheetName = biffVersion switch
            {
                8 => new XlsShortUnicodeString(bytes, ContentOffset + 6),
                5 => new XlsShortByteString(bytes, ContentOffset + 6),
                _ => throw new ArgumentException("Unexpected BIFF version " + biffVersion, nameof(biffVersion))
            };
        }

        internal XlsBiffBoundSheet(uint startOffset, SheetType type, SheetVisibility visibleState, string name)
            : base(new byte[32])
        {
            StartOffset = startOffset;
            Type = type;
            VisibleState = visibleState;
            m_SheetName = new XlsInternalString(name);
        }

        public enum SheetType : byte
        {
            Worksheet = 0x0,
            MacroSheet = 0x1,
            Chart = 0x2,

            // ReSharper disable once InconsistentNaming
            VBModule = 0x6
        }

        public enum SheetVisibility : byte
        {
            Visible = 0x0,
            Hidden = 0x1,
            VeryHidden = 0x2
        }

        /// <summary>
        /// Gets the worksheet data start offset.
        /// </summary>
        public uint StartOffset { get; }

        /// <summary>
        /// Gets the worksheet type.
        /// </summary>
        public SheetType Type { get; }

        /// <summary>
        /// Gets the visibility of the worksheet.
        /// </summary>
        public SheetVisibility VisibleState { get; }

        /// <summary>
        /// Gets the name of the worksheet.
        /// </summary>
        public string GetSheetName(Encoding encoding)
        {
            return m_SheetName.GetValue(encoding);
        }
    }
}