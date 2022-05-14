﻿﻿﻿﻿namespace Clover
{
    // 表格回调函数
    public delegate void RecordChangeFunction(ActorID self, string record, RecOPType opType, int row, int col,
        IVarList old);

    // 表格变化延迟回调函数
    public delegate void RecordLateChangeFunction(ActorID self, string record);

    // 逻辑类创建回调函数
    public delegate void LogicClassCallBack(int classIndex);
    
}