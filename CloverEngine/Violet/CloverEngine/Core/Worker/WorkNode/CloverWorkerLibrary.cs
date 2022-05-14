// ******************************************************************
//       /\ /|              
//       \ V/        @author     BuXinYuan 935160739@qq.com
//       | "")       
//       /  |        @Modified   2022-5-14 3:55:33            
//      /  \\       
//    *(__\_\        @Copyright  Copyright (c) BuXinYuan
// ******************************************************************

using System.Collections.Generic;

namespace Clover
{
    /// <summary>
    /// 工作序列库
    /// </summary>
    internal class CloverWorkerLibrary : KernelUnit
    {
        static readonly HashSet<WorkNode> m_RunningNodes = new HashSet<WorkNode>();
        public static readonly PolyObjectPool<WorkNode> s_NodePool = new PolyObjectPool<WorkNode>();

        /// <summary>
        /// 开启串行工作序列
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Sequence<T>(string name) where T : SequenceNode, new()
        {
            if (string.IsNullOrEmpty(name))
            {
                Log.InternalError("Sequence name is null or empty");
                return default;
            }

            T sequence = Alloc<T>();
            sequence.WorkerName = name;
            return sequence;
        }

        /// <summary>
        /// 开启串行工作序列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Sequence<T>() where T : SequenceNode, new()
        {
            T sequence = Alloc<T>();
            sequence.WorkerName = string.Empty;
            return sequence;
        }

        /// <summary>
        /// 添加串行分支
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="condition"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T SetBranch<T>(T sequence, ConditionNode condition)
            where T : SequenceNode
        {
            if (condition == null)
            {
                return sequence;
            }

            sequence.Append(condition);
            return sequence;
        }

        /// <summary>
        /// 添加串行分支
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public SequenceNode SetBranch(SequenceNode sequence, ConditionNode condition)
        {
            if (condition == null)
            {
                return sequence;
            }

            sequence.Append(condition);
            return sequence;
        }

        /// <summary>
        /// 开启并行工作序列
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Parallel<T>(string name) where T : ParallelNode, new()
        {
            if (string.IsNullOrEmpty(name))
            {
                Log.InternalError("Parallel name is null or empty");
                return default;
            }

            T parallel = Alloc<T>();
            parallel.WorkerName = name;
            return parallel;
        }

        /// <summary>
        /// 开启并行工作序列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Parallel<T>() where T : ParallelNode, new()
        {
            T parallel = Alloc<T>();
            parallel.WorkerName = string.Empty;
            return parallel;
        }

        public override void Update(float deltaTime)
        {
            foreach (WorkNode node in m_RunningNodes)
            {
                if (!node.isDone)
                {
                    continue;
                }

                node.End();
                Log.InternalInfo($"Release Worker : {node.WorkerName}");
                Release(node);
            }
        }

        internal static T Alloc<T>() where T : WorkNode, new()
        {
            T node = s_NodePool.Get<T>();
            m_RunningNodes.Add(node);
            return node;
        }

        internal static void Release<T>(T node) where T : WorkNode
        {
            s_NodePool.Release(node);
        }
    }
}