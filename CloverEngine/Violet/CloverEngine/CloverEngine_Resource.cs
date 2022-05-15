using UnityEngine;

namespace Clover
{
    public static partial class CloverEngine
    {
        /// <summary>
        /// 获取全局根节点
        /// </summary>
        /// <param name="resType"></param>
        /// <returns></returns>
        public static ActorID GetGlobalRootActor(EResType resType)
        {
            CloverActorHierarchy hierarchy = Kernel.Get<CloverActorHierarchy>();
            if (hierarchy != null)
            {
                return hierarchy.GetRootActor(resType);
            }

            Log.UnexpectedError(nameof(CloverActorHierarchy));
            return CloverActorHierarchy.Null.ID;
        }

        /// <summary>
        /// 获取全局根节点
        /// </summary>
        /// <param name="resType"></param>
        /// <returns></returns>
        public static Transform GetGlobalRoot(EResType resType)
        {
            CloverActorHierarchy hierarchy = Kernel.Get<CloverActorHierarchy>();
            if (hierarchy != null)
            {
                return hierarchy.GetRoot(resType);
            }

            Log.UnexpectedError(nameof(CloverActorHierarchy));
            return CloverActorHierarchy.Null.transform;
        }

        /// <summary>
        /// 获取全局根节点组件
        /// </summary>
        /// <param name="resType"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetGlobalRootComponent<T>(EResType resType) where T : Component
        {
            CloverActorHierarchy hierarchy = Kernel.Get<CloverActorHierarchy>();
            if (hierarchy != null)
            {
                var root = hierarchy.GetRoot(resType);
                if (root == null)
                {
                    Log.UnexpectedError(nameof(CloverActorHierarchy));
                    return default;
                }

                T component = root.GetComponent<T>();
                if (component == null)
                {
                    component = root.gameObject.AddComponent<T>();
                }

                return component;
            }

            Log.UnexpectedError(nameof(CloverActorHierarchy));
            return CloverActorHierarchy.Null.transform.GetComponent<T>();
        }
    }
}