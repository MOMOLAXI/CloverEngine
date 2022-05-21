using System;
using System.Collections.Generic;
using System.IO;
using Clover;

namespace Clover
{
    internal class CompoundStream : Stream
    {
        public CompoundStream(CompoundDocument document, Stream baseStream, List<uint> sectorChain, int length, bool leaveOpen)
        {
            Document = document;
            BaseStream = baseStream;
            IsMini = false;
            LeaveOpen = leaveOpen;
            Length = length;
            SectorChain = sectorChain;
            ReadSector();
        }

        public CompoundStream(CompoundDocument document, Stream baseStream, uint baseSector, int length, bool isMini, bool leaveOpen)
        {
            Document = document;
            BaseStream = baseStream;
            IsMini = isMini;
            Length = length;
            LeaveOpen = leaveOpen;

            if (IsMini)
            {
                SectorChain = Document.GetSectorChain(baseSector, Document.MiniSectorTable);
                RootSectorChain = Document.GetSectorChain(Document.RootEntry.StreamFirstSector, Document.SectorTable);
            }
            else
            {
                SectorChain = Document.GetSectorChain(baseSector, Document.SectorTable);
            }

            ReadSector();
        }

        public List<uint> SectorChain { get; }

        public List<uint> RootSectorChain { get; }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length { get; }

        public override long Position { get => Offset - SectorBytes.Length + SectorOffset; set => Seek(value, SeekOrigin.Begin); }

        private Stream BaseStream { get; set; }

        private CompoundDocument Document { get; }

        private bool IsMini { get; }

        private bool LeaveOpen { get; }

        private int SectorChainOffset { get; set; }

        private int Offset { get; set; }

        private int SectorOffset { get; set; }

        private byte[] SectorBytes { get; set; }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int index = 0;
            while (index < count && Position < Length)
            {
                if (SectorOffset == SectorBytes.Length)
                {
                    ReadSector();
                    SectorOffset = 0;
                }

                int chunkSize = Math.Min(count - index, SectorBytes.Length - SectorOffset);
                Array.Copy(SectorBytes, SectorOffset, buffer, offset + index, chunkSize);
                index += chunkSize;
                SectorOffset += chunkSize;
            }

            return index;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            int sectorSize = IsMini ? Document.Header.MiniSectorSize : Document.Header.SectorSize;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    SectorChainOffset = (int)(offset / sectorSize);
                    Offset = SectorChainOffset * sectorSize;
                    SectorOffset = (int)(offset % sectorSize);
                    if (Offset < Length)
                        ReadSector();
                    return Position;
                case SeekOrigin.Current:
                    return Seek(Position + offset, SeekOrigin.Begin);
                case SeekOrigin.End:
                    return Seek(Length + offset, SeekOrigin.Begin);
                default:
                    return Offset;
            }
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !LeaveOpen)
            {
                BaseStream?.Dispose();
                BaseStream = null;
            }

            base.Dispose(disposing);
        }

        private void ReadSector()
        {
            if (IsMini)
            {
                ReadMiniSector();
            }
            else
            {
                ReadRegularSector();
            }
        }

        private void ReadMiniSector()
        {
            uint sector = SectorChain[SectorChainOffset];
            int miniStreamOffset = (int)Document.GetMiniSectorOffset(sector);

            int rootSectorIndex = miniStreamOffset / Document.Header.SectorSize;
            if (rootSectorIndex >= RootSectorChain.Count)
            {
                throw new CompoundDocumentException(Errors.ErrorEndOfFile);
            }

            uint rootSector = RootSectorChain[rootSectorIndex];
            int rootOffset = miniStreamOffset % Document.Header.SectorSize;

            BaseStream.Seek(Document.GetSectorOffset(rootSector) + rootOffset, SeekOrigin.Begin);

            int chunkSize = (int)Math.Min(Length - Offset, Document.Header.MiniSectorSize);
            SectorBytes = new byte[chunkSize];
            if (BaseStream.Read(SectorBytes, 0, chunkSize) < chunkSize)
            {
                throw new CompoundDocumentException(Errors.ErrorEndOfFile);
            }

            Offset += chunkSize;
            SectorChainOffset++;
        }

        private void ReadRegularSector()
        {
            uint sector = SectorChain[SectorChainOffset];
            BaseStream.Seek(Document.GetSectorOffset(sector), SeekOrigin.Begin);

            int chunkSize = (int)Math.Min(Length - Offset, Document.Header.SectorSize);
            SectorBytes = new byte[chunkSize];
            if (BaseStream.Read(SectorBytes, 0, chunkSize) < chunkSize)
            {
                throw new CompoundDocumentException(Errors.ErrorEndOfFile);
            }

            Offset += chunkSize;
            SectorChainOffset++;
        }
    }
}
