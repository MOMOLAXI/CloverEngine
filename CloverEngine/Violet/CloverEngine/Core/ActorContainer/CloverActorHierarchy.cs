﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Clover
{
    internal class CloverActorHierarchy : KernelUnit
    {
        const string ACTOR_NAME_FORMAT = "{0}{1}";
        
        /// <summary>
        /// 当前场景
        /// </summary>
        Actor CurScene { get; set; }

        /// <summary>
        /// 局部缓存ID
        /// </summary>
        ActorID m_L1CachedId;

        /// <summary>
        /// 局部缓存Actor
        /// </summary>
        Actor m_L1CachedObj;

        /// <summary>
        /// 实例
        /// </summary>
        readonly RapidList<Actor> m_Instance = new RapidList<Actor>();

        /// <summary>
        /// Actor 查询
        /// </summary>
        readonly Dictionary<long, int> m_ActorIDMapping = new Dictionary<long, int>();

        /// <summary>
        /// 空对象
        /// </summary>
        public static Actor Null { get; set; }

        /// <summary>
        /// 引擎
        /// </summary>
        public static Actor Engine { get; set; }

        readonly Dictionary<EResType, Actor> m_GlobalRoots = new Dictionary<EResType, Actor>();

        /// <summary>
        /// 当前场景ID
        /// </summary>
        public ActorID CurSceneID => CurScene != null ? CurScene.ID : Null.ID;

        CloverClassEventLibrary m_EventLibrary;
        CloverPropertyLibrary m_PropertyLibrary;

        /// <summary>
        /// Initialize New World
        /// </summary>
        public override void OnBuild()
        {
            base.OnBuild();

            m_EventLibrary = Kernel.Get<CloverClassEventLibrary>();
            if (m_EventLibrary == null)
            {
                Log.InternalError("CloverClassEventLibrary must construct before CloverActorHierarchy");
                return;
            }

            m_PropertyLibrary = Kernel.Get<CloverPropertyLibrary>();
            if (m_PropertyLibrary == null)
            {
                Log.InternalError("CloverPropertyLibrary must construct before CloverActorHierarchy");
                return;
            }

            //空对象
            Null = new GameObject("Template").AddComponent<Actor>();
            Null.ClassName = "Template";
            Null.SetEventContext(ClassEventContext.Create());
            Object.DontDestroyOnLoad(Null.gameObject);

            //引擎根节点
            Engine = new GameObject(nameof(CloverEngine)).AddComponent<Actor>();
            Engine.ClassName = nameof(CloverEngine);
            Engine.SetEventContext(ClassEventContext.Create());
            Object.DontDestroyOnLoad(Engine);

            //构造全局根节点
            for (EResType i = EResType.Actor; i < EResType.Max; ++i)
            {
                Actor root = CreateActor(i.ToString(), Engine, i);
                m_GlobalRoots[i] = root;
            }
        }

        /// <summary>
        /// 获取全局根节点
        /// </summary>
        /// <param name="resType"></param>
        /// <returns></returns>
        public Transform GetRoot(EResType resType)
        {
            if (resType == EResType.CloverEngine)
            {
                return Engine.transform;
            }

            if (m_GlobalRoots.TryGetValue(resType, out Actor actor))
            {
                return actor.transform;
            }

            return Null.transform;
        }

        /// <summary>
        /// 创建场景
        /// </summary>
        public ActorID CreateScene()
        {
            try
            {
                CurScene = CreateActor("Scene", null, EResType.Scene);
            }
            catch (Exception e)
            {
                Log.InternalException(e);
            }

            return CurScene.ID;
        }

        /// <summary>
        /// 查找Actor
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public Actor Find(ActorID actorId)
        {
            if (actorId == ActorID.zero)
            {
                return Null;
            }

            if (actorId == m_L1CachedId)
            {
                return m_L1CachedObj;
            }

            if (!m_ActorIDMapping.TryGetValue(actorId, out int index))
            {
                return Null;
            }

            Actor actor = m_Instance[index];
            m_L1CachedId = actorId;
            m_L1CachedObj = actor;
            return actor;
        }

        /// <summary>
        /// 创建Actor
        /// </summary>
        /// <param name="className"></param>
        /// <param name="parent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Actor CreateActor(string className, Actor parent = null, EResType type = EResType.Actor)
        {
            if (string.IsNullOrEmpty(className))
            {
                return Null;
            }

            if (parent == null)
            {
                if (m_GlobalRoots.TryGetValue(type, out Actor root))
                {
                    parent = root;
                }
            }

            GameObject go = parent == null
                ? Object.Instantiate(Null.gameObject)
                : Object.Instantiate(Null.gameObject, parent.transform, true);

            ActorID id = ActorIDHelper.Generate(type);

            go.name = ACTOR_NAME_FORMAT.SafeFormat(id, className);

            Actor actor = go.GetComponent<Actor>();
            if (actor == null)
            {
                actor = go.AddComponent<Actor>();
            }

            actor.ID = id;
            actor.ClassName = className;
            actor.OnDestroyActor = OnDestroyActor;
            actor.SetEventContext(m_EventLibrary.Alloc(className));
            actor.SendClassEvent(EClassEvent.OnCreate, id, VarList.AllocAutoVarList());

            PropertyCollection property = m_PropertyLibrary.Alloc();
            actor.InitializeProperty(property);

            if (parent != null)
            {
                parent.AddChild(actor);
            }

            m_Instance.Add(actor);
            m_ActorIDMapping[actor.ID] = m_Instance.Count - 1;
            return actor;
        }

        public override void Update(float deltaTime)
        {
            Actor[] buffer = m_Instance.Buffer;
            int count = m_Instance.Count;

            for (int i = 0; i < count; ++i)
            {
                Actor actor = buffer[i];
                if (actor == null)
                {
                    continue;
                }

                try
                {
                    actor.OnUpdate();
                }
                catch (Exception ex)
                {
                    Log.InternalException(ex);
                }
            }
        }

        public override void LateUpdate()
        {
            Actor[] buffer = m_Instance.Buffer;
            int count = m_Instance.Count;

            for (int i = 0; i < count; ++i)
            {
                Actor actor = buffer[i];
                if (actor == null)
                {
                    continue;
                }

                try
                {
                    actor.OnLateUpdate();
                }
                catch (Exception ex)
                {
                    Log.InternalException(ex);
                }
            }
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="actor"></param>
        void OnDestroyActor(Actor actor)
        {
            if (m_ActorIDMapping.TryGetValue(actor.ID, out int index))
            {
                m_Instance.RemoveAt(index);
                m_ActorIDMapping.Remove(actor.ID);
            }

            //清理查找缓存
            if (actor.ID == m_L1CachedId)
            {
                m_L1CachedId = ActorID.zero;
                m_L1CachedObj = Null;
            }

            //释放类事件上下文
            m_EventLibrary.Release(actor.EventContext);

            //释放属性列表
            CloverPropertyLibrary propertyLibrary = Kernel.Get<CloverPropertyLibrary>();
            if (propertyLibrary == null)
            {
                Log.UnexpectedError(nameof(OnDestroyActor));
                return;
            }

            propertyLibrary.Release(actor.Properties);
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="immediate"></param>
        public void DestroyActor(ActorID actorId, bool immediate)
        {
            Actor actor = Find(actorId);
            if (actor == Null)
            {
                return;
            }

            DestroyActor(actor, immediate);
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="immediate"></param>
        public void DestroyActor(Actor actor, bool immediate)
        {
            if (actor == null)
            {
                return;
            }

            actor.OnDestroySelf();

            if (immediate)
            {
                Object.DestroyImmediate(actor.gameObject);
            }
            else
            {
                Object.Destroy(actor.gameObject);
            }
        }
    }
}