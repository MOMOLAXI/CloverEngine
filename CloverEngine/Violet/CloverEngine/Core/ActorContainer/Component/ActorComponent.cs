﻿namespace Clover
{
    public abstract class ActorComponent : ICacheAble
    {
        bool m_IsActive = true;
        protected ActorID m_Self;

        // 高频度调用属性，不使用属性方法
        internal bool isStarted;
        internal bool isDestroyed;

        public ActorID self => m_Self;

        public bool isActive
        {
            get => m_IsActive;
            set
            {
                if (m_IsActive == value)
                {
                    return;
                }

                m_IsActive = value;

                try
                {
                    if (m_IsActive)
                    {
                        OnEnable();
                    }
                    else
                    {
                        OnDisable();
                    }
                }
                catch (System.Exception ex)
                {
                    Log.InternalException(ex);
                }
            }
        }

        internal void Awake()
        {
            OnAwake();
        }

        internal void Start()
        {
            OnStart();
        }

        internal void Update()
        {
            OnUpdate();
        }

        internal void LateUpdate()
        {
            OnLateUpdate();
        }

        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnLateUpdate() { }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void OnDestroy() { }

        public void DestroySelf()
        {
            if (isDestroyed)
            {
                return;
            }

            isActive = false;
            isDestroyed = true;

            try
            {
                OnDestroy();
            }
            catch (System.Exception ex)
            {
                Log.InternalException(ex);
            }
        }

        internal void Init(ActorID id)
        {
            m_Self = id;
            m_IsActive = true;
            isStarted = false;
            isDestroyed = false;
        }

        public bool cacheFlag { get; set; }
        public virtual void Reset() { }
    }
}