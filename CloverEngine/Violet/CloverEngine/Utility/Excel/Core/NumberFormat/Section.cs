using System.Collections.Generic;

namespace Clover
{
    internal class Section
    {
        public SectionType Type { get; set; }

        public ExcelColor ExcelColor { get; set; }

        public Condition Condition { get; set; }

        public ExponentialSection Exponential { get; set; }

        public FractionSection Fraction { get; set; }

        public DecimalSection Number { get; set; }

        public List<string> GeneralTextDateDurationParts { get; set; }
    }
}