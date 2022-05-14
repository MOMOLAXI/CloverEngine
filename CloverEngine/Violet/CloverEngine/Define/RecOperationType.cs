﻿﻿﻿﻿namespace Clover
{
    public enum RecOPType
    {
        /// <summary>
        /// 表格添加新的行
        /// </summary>
        AddRow,

        /// <summary>
        /// 表格移除行
        /// </summary>
        RemoveRow,

        /// <summary>
        /// 表格数据改变
        /// </summary>
        ValueChange,

        /// <summary>
        /// 表格行数据改变，该操作可能和ValueChange操作同时触发，具体触发参照服务器的操作
        /// </summary>
        RowValueChange,

        /// <summary>
        /// 清空表格
        /// </summary>
        Clear,
    }
}