namespace Clover
{
    public static partial class CloverEngine
    {
        /// <summary>
        /// 开启串行工作序列
        /// </summary>
        /// <returns></returns>
        public static SequenceNode Sequence()
        {
            return CloverWorkerLibrary.Sequence<SequenceNode>();
        }

        /// <summary>
        /// 开启串行工作序列
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static SequenceNode Sequence(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Log.InternalError($"sequence name is null or empty");
                return default;
            }

            return CloverWorkerLibrary.Sequence<SequenceNode>(name);
        }

        /// <summary>
        /// 添加串行分支
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static SequenceNode Condition(this SequenceNode sequence, ConditionNode condition)
        {
            if (condition == null)
            {
                return sequence;
            }

            sequence.Append(condition);
            return sequence;
        }

        /// <summary>
        /// 开启串行工作序列
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static SequenceNode Append(this SequenceNode sequence, WorkNode node)
        {
            if (node == null)
            {
                return sequence;
            }

            sequence.Append(node);
            return sequence;
        }

        /// <summary>
        /// 开启串行工作序列
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static SequenceNode Append(this SequenceNode sequence, params WorkNode[] args)
        {
            if (args == null)
            {
                return sequence;
            }

            for (int i = 0; i < args.Length; i++)
            {
                WorkNode node = args[i];
                if (node == null)
                {
                    continue;
                }

                sequence.Append(node);
            }

            return sequence;
        }

        /// <summary>
        /// 串行工作中开启一段并行序列
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static SequenceNode Parallel(this SequenceNode sequence, params WorkNode[] args)
        {
            if (args == null)
            {
                return sequence;
            }

            ParallelNode parallelNode = Paralle(string.Empty);
            for (int i = 0; i < args.Length; i++)
            {
                WorkNode node = args[i];
                if (node == null)
                {
                    continue;
                }

                parallelNode.Set(node);
            }

            sequence.Append(parallelNode);

            return sequence;
        }

        /// <summary>
        /// 开启并行工作序列
        /// </summary>
        /// <returns></returns>
        public static ParallelNode Paralle()
        {
            return CloverWorkerLibrary.Parallel<ParallelNode>();
        }

        /// <summary>
        /// 开启并行工作序列
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ParallelNode Paralle(string name)
        {
            return CloverWorkerLibrary.Parallel<ParallelNode>(name);
        }


        /// <summary>
        /// 添加并行工作节点
        /// </summary>
        /// <param name="parallel"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ParallelNode Set(this ParallelNode parallel, WorkNode node)
        {
            if (node == null)
            {
                return parallel;
            }

            parallel.Set(node);
            return parallel;
        }

        /// <summary>
        /// 添加并行工作节点
        /// </summary>
        /// <param name="parallel"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static ParallelNode Set(this ParallelNode parallel, params WorkNode[] args)
        {
            if (args == null)
            {
                return parallel;
            }

            for (int i = 0; i < args.Length; i++)
            {
                WorkNode node = args[i];
                if (node == null)
                {
                    continue;
                }

                parallel.Set(node);
            }

            return parallel;
        }
    }
}