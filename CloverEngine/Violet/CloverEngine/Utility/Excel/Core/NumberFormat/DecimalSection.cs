using System.Collections.Generic;

namespace Clover
{
    internal class DecimalSection
    {
        public bool ThousandSeparator { get; set; }

        public double ThousandDivisor { get; set; }

        public double PercentMultiplier { get; set; }

        public List<string> BeforeDecimal { get; set; }

        public bool DecimalSeparator { get; set; }

        public List<string> AfterDecimal { get; set; }

        public static bool TryParse(List<string> tokens, out DecimalSection format)
        {
            if (Parser.ParseNumberTokens(tokens, 0, out List<string> beforeDecimal, out bool decimalSeparator, out List<string> afterDecimal) == tokens.Count)
            {
                bool thousandSeparator;
                double divisor = GetTrailingCommasDivisor(tokens, out thousandSeparator);
                double multiplier = GetPercentMultiplier(tokens);

                format = new DecimalSection()
                {
                    BeforeDecimal = beforeDecimal,
                    DecimalSeparator = decimalSeparator,
                    AfterDecimal = afterDecimal,
                    PercentMultiplier = multiplier,
                    ThousandDivisor = divisor,
                    ThousandSeparator = thousandSeparator
                };

                return true;
            }

            format = null;
            return false;
        }

        private static double GetPercentMultiplier(List<string> tokens)
        {
            // If there is a percentage literal in the part list, multiply the result by 100
            foreach (string token in tokens)
            {
                if (token == "%")
                    return 100;
            }

            return 1;
        }

        private static double GetTrailingCommasDivisor(List<string> tokens, out bool thousandSeparator)
        {
            // This parses all comma literals in the part list:
            // Each comma after the last digit placeholder divides the result by 1000.
            // If there are any other commas, display the result with thousand separators.
            bool hasLastPlaceholder = false;
            double divisor = 1.0;

            for (int j = 0; j < tokens.Count; j++)
            {
                int tokenIndex = tokens.Count - 1 - j;
                string token = tokens[tokenIndex];

                if (!hasLastPlaceholder)
                {
                    if (Token.IsPlaceholder(token))
                    {
                        // Each trailing comma multiplies the divisor by 1000
                        for (int k = tokenIndex + 1; k < tokens.Count; k++)
                        {
                            token = tokens[k];
                            if (token == ",")
                                divisor *= 1000.0;
                            else
                                break;
                        }

                        // Continue scanning backwards from the last digit placeholder, 
                        // but now look for a thousand separator comma
                        hasLastPlaceholder = true;
                    }
                }
                else
                {
                    if (token == ",")
                    {
                        thousandSeparator = true;
                        return divisor;
                    }
                }
            }

            thousandSeparator = false;
            return divisor;
        }
    }
}