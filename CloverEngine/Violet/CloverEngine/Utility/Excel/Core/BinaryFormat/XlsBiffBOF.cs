namespace Clover
{
    /// <summary>
    /// Represents BIFF BOF record
    /// </summary>
    internal class XlsBiffBOF : XlsBiffRecord
    {
        internal XlsBiffBOF(byte[] bytes)
            : base(bytes)
        {
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public ushort Version => ReadUInt16(0x0);

        /// <summary>
        /// Gets the type of the BIFF block
        /// </summary>
        public BIFFTYPE Type => (BIFFTYPE)ReadUInt16(0x2);

        /// <summary>
        /// Gets the creation Id.
        /// </summary>
        /// <remarks>Not used before BIFF5</remarks>
        public ushort CreationId => RecordSize < 6 ? (ushort) 0 : ReadUInt16(0x4);

        /// <summary>
        /// Gets the creation year.
        /// </summary>
        /// <remarks>Not used before BIFF5</remarks>
        public ushort CreationYear => RecordSize < 8 ? (ushort) 0 : ReadUInt16(0x6);

        /// <summary>
        /// Gets the file history flag.
        /// </summary>
        /// <remarks>Not used before BIFF8</remarks>
        public uint HistoryFlag => RecordSize < 12 ? (uint) 0 : ReadUInt32(0x8);

        /// <summary>
        /// Gets the minimum Excel version to open this file.
        /// </summary>
        /// <remarks>Not used before BIFF8</remarks>
        public uint MinVersionToOpen => RecordSize < 16 ? (uint) 0 : ReadUInt32(0xC);
    }
}