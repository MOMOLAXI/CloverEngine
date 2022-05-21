using System;
using System.Collections.Generic;
using System.Text;

namespace Clover
{
    /// <summary>
    /// Represents a Shared String Table in BIFF8 format
    /// </summary>
    internal class XlsBiffSST : XlsBiffRecord
    {
        private readonly List<IXlsString> m_XlsStrings;
        private readonly XlsSSTReader m_XlsSstReader = new XlsSSTReader();

        internal XlsBiffSST(byte[] bytes)
            : base(bytes)
        {
            m_XlsStrings = new List<IXlsString>();
            ReadSstStrings();
        }

        /// <summary>
        /// Gets the number of strings in SST
        /// </summary>
        public uint Count => ReadUInt32(0x0);

        /// <summary>
        /// Gets the count of unique strings in SST
        /// </summary>
        public uint UniqueCount => ReadUInt32(0x4);

        /// <summary>
        /// Parses strings out of a Continue record
        /// </summary>
        public void ReadContinueStrings(XlsBiffContinue sstContinue)
        {
            if (m_XlsStrings.Count == UniqueCount)
            {
                return;
            }

            foreach (IXlsString str in m_XlsSstReader.ReadStringsFromContinue(sstContinue))
            {
                m_XlsStrings.Add(str);

                if (m_XlsStrings.Count == UniqueCount)
                {
                    break;
                }
            }
        }

        public void Flush()
        {
            IXlsString str = m_XlsSstReader.Flush();
            if (str != null)
            {
                m_XlsStrings.Add(str);
            }
        }

        /// <summary>
        /// Returns string at specified index
        /// </summary>
        /// <param name="sstIndex">Index of string to get</param>
        /// <param name="encoding">Workbook encoding</param>
        /// <returns>string value if it was found, empty string otherwise</returns>
        public string GetString(uint sstIndex, Encoding encoding)
        {
            if (sstIndex < m_XlsStrings.Count)
                return m_XlsStrings[(int)sstIndex].GetValue(encoding);

            return null; // #VALUE error
        }

        /// <summary>
        /// Parses strings out of this SST record
        /// </summary>
        private void ReadSstStrings()
        {
            if (m_XlsStrings.Count == UniqueCount)
            {
                return;
            }

            foreach (IXlsString str in m_XlsSstReader.ReadStringsFromSST(this))
            {
                m_XlsStrings.Add(str);

                if (m_XlsStrings.Count == UniqueCount)
                {
                    break;
                }
            }
        }
    }
}