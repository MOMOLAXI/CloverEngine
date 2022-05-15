using System;
using UnityEngine;

namespace Clover
{
    /// <summary>
    /// 工作节点基础类
    /// </summary>
    public class WorkNode : ICacheAble
    {
        static ulong m_Serial;
        string m_WorkerName;

        /// <summary>
        /// 节点名称
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// 工作名称
        /// </summary>
        public string WorkerName
        {
            get => string.IsNullOrEmpty(Name) ? m_WorkerName : Name;
            set
            {
                if (string.IsNullOrEmpty(Name))
                {
                    m_WorkerName = string.IsNullOrEmpty(value) ? $"WorkerNode-{m_Serial}" : $"WorkerNode-{value}";
                }
                else
                {
                    m_WorkerName = string.IsNullOrEmpty(value) ? $"WorkerNode-{Name}" : $"WorkerNode-{value}";
                }
            }
        }

        float m_StartTime;

        /// <summary>
        /// 是否工作结束
        /// </summary>
        public virtual bool isDone => false;

        internal CloverKernel Kernel { get; set; }

        public WorkNode()
        {
            m_Serial++;
        }

        /// <summary>
        /// 启动
        /// </summary>
        protected virtual void OnStart() { }

        /// <summary>
        /// 结束
        /// </summary>
        protected virtual void OnEnd() { }

        public void Start()
        {
            m_StartTime = Time.time;
            WorkerName = Name;
            Log.Info($"[{WorkerName}::Start] " +
                     $" Time : {Time.time}s ");
            OnStart();
        }

        public void End()
        {
            long startTicks = DateTime.Now.Ticks;

            OnEnd();

            float costTime = Time.time - m_StartTime;

            Log.Info($"[{WorkerName}::OnEnd] " +
                     $" Time : {Time.time}s" +
                     $" Cost : {costTime}s" +
                     $" EndCost : {(DateTime.Now.Ticks - startTicks) / 10000}ms");
        }

        protected virtual void ResetToCache() { }

        public bool cacheFlag { get; set; }

        public void Reset()
        {
            ResetToCache();
        }
    }
}