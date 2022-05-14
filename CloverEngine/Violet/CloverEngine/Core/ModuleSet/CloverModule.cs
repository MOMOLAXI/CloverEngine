﻿﻿namespace Clover
{
    public abstract class TLogicModule<T> : CloverModule where T : new()
    {
        public static T S = new T();
    }

    public abstract class CloverModule
    {
        public string name => GetType().FullName;

        internal void Init()
        {
            OnInit();
        }

        internal void ShutDown()
        {
            OnShutDown();
        }

        internal void Reset()
        {
            OnReset();
        }

        internal void Update()
        {
            OnUpdate();
        }

        internal void LateUpdate()
        {
            OnLateUpdate();
        }

        internal void Background()
        {
            OnBackground();
        }

        internal void Foreground()
        {
            OnForeground();
        }

        /// <summary>
        /// 模块初始化，在游戏开始时调用，全游戏调用一次
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// 模块关闭，在游戏退出时调用，全游戏调用一次
        /// </summary>
        protected virtual void OnShutDown() { }

        /// <summary>
        /// 回登陆界面时调用
        /// </summary>
        protected virtual void OnReset() { }

        /// <summary>
        /// 帧调用
        /// </summary>
        protected virtual void OnUpdate() { }

        /// <summary>
        /// 帧调用
        /// </summary>
        protected virtual void OnLateUpdate() { }

        /// <summary>
        /// 由前台切到后台时调用
        /// </summary>
        protected virtual void OnBackground() { }

        /// <summary>
        /// 由后台切到前台时调用
        /// </summary>
        protected virtual void OnForeground() { }
    }

    public enum GlobalResLoadStage
    {
        InSplash, //闪屏开始之后, 闪屏完成之前
        BeforeLogin, //闪屏结束之后, 用户登录之前，
        BeforeSceneReady, //第一次场景创建开始AddEventHook("scene", LogicEvent.OnCreate, OnSceneCreate)之后

        //第一次场景加载完成m_Kernel.AddDynamicCommand(CommandID.SceneReady, OnSceneReady)之前
        Max,
    }

    public interface IGlobalResModule
    {
        // 开始加载
        void OnStartLoad(GlobalResLoadStage stage);

        // 是否加载完成
        bool IsLoaded(GlobalResLoadStage stage);

        // 清理
        void OnClean(GlobalResLoadStage stage);

        // 收集依赖列表
        IGlobalResModule[] OnCollectDepends(GlobalResLoadStage stage);
    }

    public class TSingleton<T> where T : TSingleton<T>, new()
    {
        private static T m_Instance;

        public static T S
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new T();
                    m_Instance.OnSingletonInit();
                }

                return m_Instance;
            }
        }

        public static T ResetInstance()
        {
            m_Instance = new T();
            m_Instance.OnSingletonInit();
            return m_Instance;
        }

        protected virtual void OnSingletonInit() { }
    }
}