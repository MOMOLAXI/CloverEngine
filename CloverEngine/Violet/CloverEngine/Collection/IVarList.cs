﻿﻿﻿﻿﻿namespace Clover
{
    public interface IVarList
    {
        int Count { get; }
        IVarList Clone();
        VarType GetVarType(int index);
        Var GetVar(int index);
        bool GetBool(int index);
        int GetInt32(int index);
        long GetInt64(int index);
        float GetFloat(int index);
        double GetDouble(int index);
        string GetString(int index);
        ActorID GetObject(int index);
        byte[] GetBinary(int index);
    }
}