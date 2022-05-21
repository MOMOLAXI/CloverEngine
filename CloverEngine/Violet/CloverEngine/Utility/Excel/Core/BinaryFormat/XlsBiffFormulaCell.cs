using System;
using System.Text;
using Clover;

namespace Clover
{
    /// <summary>
    /// Represents a cell containing formula
    /// </summary>
    internal class XlsBiffFormulaCell : XlsBiffBlankCell
    {
        readonly int m_BiffVersion;
        bool m_BooleanValue;
        CellError m_ErrorValue;
        double m_XNumValue;
        FormulaValueType m_FormulaValueType;
        bool m_Initialized;

        internal XlsBiffFormulaCell(byte[] bytes, int biffVersion)
            : base(bytes)
        {
            m_BiffVersion = biffVersion;
        }

        [Flags]
        public enum FormulaFlags : ushort
        {
            AlwaysCalc = 0x0001,
            CalcOnLoad = 0x0002,
            SharedFormulaGroup = 0x0008
        }

        public enum FormulaValueType
        {
            Unknown,

            /// <summary>
            /// Indicates that a string value is stored in a String record that immediately follows this record. See[MS - XLS] 2.5.133 FormulaValue.
            /// </summary>
            String,

            /// <summary>
            /// Indecates that the formula value is an empty string.
            /// </summary>
            EmptyString,

            /// <summary>
            /// Indicates that the <see cref="BooleanValue"/> property is valid.
            /// </summary>
            Boolean,

            /// <summary>
            /// Indicates that the <see cref="ErrorValue"/> property is valid.
            /// </summary>
            Error,

            /// <summary>
            /// Indicates that the <see cref="XNumValue"/> property is valid.
            /// </summary>
            Number
        }

        /// <summary>
        /// Gets the formula value type.
        /// </summary>
        public FormulaValueType FormulaType
        {
            get
            {
                LazyInit();
                return m_FormulaValueType;
            }
        }

        public bool BooleanValue
        {
            get
            {
                LazyInit();
                return m_BooleanValue;
            }
        }

        public CellError ErrorValue
        {
            get
            {
                LazyInit();
                return m_ErrorValue;
            }
        }

        public double XNumValue
        {
            get
            {
                LazyInit();
                return m_XNumValue;
            }
        }

        /*
        public FormulaFlags Flags
        {
            get
            {
                LazyInit();
                return _flags;
            }
        }
        */

        private void LazyInit()
        {
            if (m_Initialized)
                return;
            m_Initialized = true;

            if (m_BiffVersion == 2)
            {
                // _flags = (FormulaFlags)ReadUInt16(0xF);
                m_XNumValue = ReadDouble(0x7);
                m_FormulaValueType = FormulaValueType.Number;
            }
            else
            {
                // _flags = (FormulaFlags)ReadUInt16(0xE);
                ushort formulaValueExprO = ReadUInt16(0xC);
                if (formulaValueExprO != 0xFFFF)
                {
                    m_FormulaValueType = FormulaValueType.Number;
                    m_XNumValue = ReadDouble(0x6);
                }
                else
                {
                    // var formulaLength = ReadByte(0xF);
                    byte formulaValueByte1 = ReadByte(0x6);
                    byte formulaValueByte3 = ReadByte(0x8);
                    switch (formulaValueByte1)
                    {
                        case 0x00:
                            m_FormulaValueType = FormulaValueType.String;
                            break;
                        case 0x01:
                            m_FormulaValueType = FormulaValueType.Boolean;
                            m_BooleanValue = formulaValueByte3 != 0;
                            break;
                        case 0x02:
                            m_FormulaValueType = FormulaValueType.Error;
                            m_ErrorValue = (CellError) formulaValueByte3;
                            break;
                        case 0x03:
                            m_FormulaValueType = FormulaValueType.EmptyString;
                            break;
                        default:
                            m_FormulaValueType = FormulaValueType.Unknown;
                            break;
                    }
                }
            }
        }
    }
}