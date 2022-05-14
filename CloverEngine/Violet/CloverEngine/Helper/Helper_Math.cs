﻿﻿﻿﻿namespace Clover
{
    public static partial class Helper
    {
        public static bool IsInCloseRange(int start, int end, int value)
        {
            return value >= start && value <= end;
        }

        public static bool IsInCloseRange(uint start, uint end, uint value)
        {
            return value >= start && value <= end;
        }

        public static bool IsInOpenRange(int start, int end, int value)
        {
            return value > start && value < end;
        }

        public static bool IsInOpenRange(uint start, uint end, uint value)
        {
            return value > start && value < end;
        }

        public static int GetNearestPowValue(int target)
        {
            if (target >= int.MaxValue)
            {
                return int.MaxValue;
            }

            int result = 1;
            while (result < target)
            {
                result *= 2;
            }

            return result;
        }
    }
}