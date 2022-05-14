namespace Clover
{
    public static partial class CloverEngine
    {
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void SubscribeDynamicMessage(ulong id, MessageEventCallback callback)
        {
            if (callback == null)
            {
                Log.InternalError($"callback is null while subscribe dynamic message for {id}");
                return;
            }

            CloverFunctionLibrary.SubscribeMessage(id, callback);
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="id"></param>
        public static void PublishDynamicMessage(ulong id)
        {
            CloverFunctionLibrary.PublishMessage(id);
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveDynamicMessage(ulong id)
        {
            CloverFunctionLibrary.RemoveMessage(id);
        }
    }
}