﻿using System.Collections.Generic;

namespace Clover
{
    internal class CloverPropertyLibrary : KernelUnit
    {
        readonly Queue<PropertyCollection> m_Cache = new Queue<PropertyCollection>();

        public override void OnBuild()
        {
            for (int i = 0; i < Const.ACTOR_INITIAL_COUNT; ++i)
            {
                PropertyCollection context = PropertyCollection.Create();
                m_Cache.Enqueue(context);
            }
        }

        /// <summary>
        /// 申请对象的属性
        /// </summary>
        /// <returns></returns>
        public PropertyCollection Alloc()
        {
            if (m_Cache.Count <= 0)
            {
                Log.InternalError($"Actor count is more than {Const.ACTOR_INITIAL_COUNT}, " +
                                  "while alloc property collection");
                return PropertyCollection.Create();
            }

            PropertyCollection collection = m_Cache.Dequeue();
            collection.Reset();
            return collection;
        }

        /// <summary>
        /// 对象删除时回收
        /// </summary>
        /// <param name="collection"></param>
        public void Release(PropertyCollection collection)
        {
            if (collection == null)
            {
                Log.InternalError(
                    $"PropertyCollection is destroyed somewhere, it shouldn't happen , plz check this out");
                return;
            }

            collection.Reset();
            m_Cache.Enqueue(collection);
        }
    }
}