namespace Clover
{
    public static partial class CloverEngine
    {
        /// <summary>
        /// 添加心跳时长函数
        /// </summary>
        /// <param name="callbackName"></param>
        /// <param name="function"></param>
        /// <param name="interval">间隔时间(秒)</param>
        /// <param name="duration">总时长(秒)</param>
        /// <returns></returns>
        public static void AddGlobalHeartBeat(
            string callbackName,
            GlobalHeartBeatFunction function,
            float interval,
            float duration = -1)
        {
            CloverHeartBeatLibrary heartBeatLibrary = Kernel.Get<CloverHeartBeatLibrary>();
            if (heartBeatLibrary == null)
            {
                Log.UnexpectedError(nameof(AddGlobalHeartBeat));
                return;
            }

            heartBeatLibrary.RegisterHeartBeat(callbackName, function, interval, duration);
        }

        /// <summary>
        /// 添加心跳计数函数
        /// </summary>
        /// <param name="callbackName"></param>
        /// <param name="function"></param>
        /// <param name="interval"></param>
        /// <param name="count"></param>
        public static void AddGlobalCountBeat(
            string callbackName,
            GlobalCountBeatFunction function,
            float interval,
            int count = -1)
        {
            CloverHeartBeatLibrary heartBeatLibrary = Kernel.Get<CloverHeartBeatLibrary>();
            if (heartBeatLibrary == null)
            {
                Log.UnexpectedError(nameof(AddGlobalCountBeat));
                return;
            }

            heartBeatLibrary.RegisterCountBeat(callbackName, function, count, interval);
        }

        /// <summary>
        /// 查找心跳函数
        /// </summary>
        /// <param name="callbackName"></param>
        /// <returns></returns>
        public static HeartBeat FindHeartBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError("callback is can not be null or empty");
                return Empty<HeartBeat>.Value;
            }

            CloverHeartBeatLibrary heartBeatLibrary = Kernel.Get<CloverHeartBeatLibrary>();
            if (heartBeatLibrary == null)
            {
                Log.UnexpectedError(nameof(FindHeartBeat));
                return Empty<HeartBeat>.Value;
            }

            return heartBeatLibrary.FindHeartBeat(callbackName);
        }

        /// <summary>
        /// 查找心跳计数函数
        /// </summary>
        /// <param name="callbackName"></param>
        /// <returns></returns>
        public static CountBeat FindCountBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError("callback is can not be null or empty");
                return Empty<CountBeat>.Value;
            }

            CloverHeartBeatLibrary heartBeatLibrary = Kernel.Get<CloverHeartBeatLibrary>();
            if (heartBeatLibrary == null)
            {
                Log.UnexpectedError(nameof(FindCountBeat));
                return Empty<CountBeat>.Value;
            }

            return heartBeatLibrary.FindCountBeat(callbackName);
        }

        /// <summary>
        /// 暂停心跳函数
        /// </summary>
        /// <param name="callbackName"></param>
        public static void PauseHeartBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError("callback is can not be null or empty");
                return;
            }

            CloverHeartBeatLibrary heartBeatLibrary = Kernel.Get<CloverHeartBeatLibrary>();
            if (heartBeatLibrary == null)
            {
                Log.UnexpectedError(nameof(PauseHeartBeat));
                return;
            }

            heartBeatLibrary.PauseHeartBeat(callbackName);
        }

        /// <summary>
        /// 暂停心跳计数函数
        /// </summary>
        /// <param name="callbackName"></param>
        public static void PauseCountBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError("callback is can not be null or empty");
                return;
            }

            CloverHeartBeatLibrary heartBeatLibrary = Kernel.Get<CloverHeartBeatLibrary>();
            if (heartBeatLibrary == null)
            {
                Log.UnexpectedError(nameof(PauseCountBeat));
                return;
            }

            heartBeatLibrary.PauseCountBeat(callbackName);
        }

        /// <summary>
        /// 移除心跳函数
        /// </summary>
        /// <param name="callbackName"></param>
        public static void RemoveHeartBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError("callback is can not be null or empty");
                return;
            }

            CloverHeartBeatLibrary heartBeatLibrary = Kernel.Get<CloverHeartBeatLibrary>();
            if (heartBeatLibrary == null)
            {
                Log.UnexpectedError(nameof(RemoveHeartBeat));
                return;
            }

            heartBeatLibrary.RemoveHeartBeat(callbackName);
        }

        /// <summary>
        /// 移除心跳计数函数
        /// </summary>
        /// <param name="callbackName"></param>
        public static void RemoveCountBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError("callback is can not be null or empty");
                return;
            }

            CloverHeartBeatLibrary heartBeatLibrary = Kernel.Get<CloverHeartBeatLibrary>();
            if (heartBeatLibrary == null)
            {
                Log.UnexpectedError(nameof(RemoveCountBeat));
                return;
            }

            heartBeatLibrary.RemoveCountBeat(callbackName);
        }
    }
}