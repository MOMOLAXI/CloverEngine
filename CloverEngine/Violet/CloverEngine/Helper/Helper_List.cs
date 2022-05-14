using System.Collections.Generic;

namespace Clover
{
    public static partial class Helper
    {
        /// <summary>
        /// 安全获取数据
        /// </summary>
        public static T SafeGetData<T>(this IReadOnlyList<T> dataList, int index)
        {
            if (index < 0 || dataList == null || index >= dataList.Count)
            {
                return default;
            }

            return dataList[index];
        }
    }
}