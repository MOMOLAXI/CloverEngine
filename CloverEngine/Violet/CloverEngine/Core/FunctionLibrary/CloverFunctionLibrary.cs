using System;
using System.Collections.Generic;

namespace Clover
{
    /// <summary>
    /// 动态消息回调
    /// </summary>
    /// <param name="args"></param>
    public delegate void MessageEventCallback(VarList args = null);

    internal class MessageEvent : ICacheAble
    {
        readonly List<MessageEventCallback> m_Callbacks = new List<MessageEventCallback>();

        public void Register(MessageEventCallback function)
        {
            if (function == null)
            {
                return;
            }

            m_Callbacks.Add(function);
        }

        public void Invoke(VarList args)
        {
            args ??= VarList.Empty;
            for (int i = 0; i < m_Callbacks.Count; i++)
            {
                MessageEventCallback callback = m_Callbacks[i];
                try
                {
                    callback?.Invoke(args);
                }
                catch (Exception e)
                {
                    Log.InternalException(e);
                }
            }
        }

        public bool cacheFlag { get; set; }

        public void Reset()
        {
            m_Callbacks.Clear();
        }
    }

    internal class CloverFunctionLibrary : KernelUnit
    {
        static readonly Dictionary<ulong, MessageEvent> m_Events = new Dictionary<ulong, MessageEvent>();

        static readonly ObjectPool<MessageEvent> m_EventPool = new ObjectPool<MessageEvent>();

        /// <summary>
        /// 注册动态消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public static void SubscribeMessage(ulong id, MessageEventCallback callback)
        {
            if (callback == null)
            {
                Log.InternalError($"callback for {id} is null. can not register in FunctionLibrary");
                return;
            }

            if (m_Events.TryGetValue(id, out MessageEvent message))
            {
                message.Register(callback);
                return;
            }

            message = m_EventPool.Get();
            message.Register(callback);
            m_Events[id] = message;
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveMessage(ulong id)
        {
            if (!m_Events.TryGetValue(id, out MessageEvent message))
            {
                Log.Warning($"Message Id : {id} is not found while remove message");
                return;
            }

            m_EventPool.Release(message);
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        public static void PublishMessage(ulong id, VarList args = null)
        {
            if (!m_Events.TryGetValue(id, out MessageEvent message))
            {
                Log.Warning($"Message Id : {id} is not found while publish message");
                return;
            }

            message.Invoke(args);
        }
    }
}