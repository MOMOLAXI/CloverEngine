﻿﻿﻿﻿using System;

 namespace Clover
{
    public static class Default
    {
        public const int IDENTITY = 0;

        public static char[] IDSpliter = {'[', ']', '-'};

        public static Action DefaultAction = () => { };

        public static Run DefaultRun = () => { };
        
        static void HeartBeat(ActorID self, float time) { }

        static void RecordLateChange(ActorID self, string record) { }

        static void RecordChange(ActorID self, string record, RecOPType optype, int row, int col, IVarList old) { }

        static void PropChange(ActorID self, string property, Var old) { }
    }
}