﻿﻿﻿﻿﻿namespace Clover
{
    public interface ICacheAble
    {
        bool cacheFlag { get; set; }
        void Reset();
    }
}