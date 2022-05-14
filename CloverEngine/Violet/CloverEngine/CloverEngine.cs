using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;

namespace Clover
{
    /// <summary>
    /// CloverEngine
    /// </summary>
    public static partial class CloverEngine
    {
        internal static readonly CloverKernel Kernel = new CloverKernel();

        #region LifeCycle

        /// <summary>
        /// 日志前缀
        /// </summary>
        public const string ENGINE_LOG_TAG = "[<color=#1E90FF>CloverEngine</color>]";

        /// <summary>
        /// 引擎根节点
        /// </summary>
        public static Transform Engine => CloverActorHierarchy.Engine.transform;

        /// <summary>
        /// Called on Awake
        /// </summary>
        public static void Start()
        {
            //构建内核
            Kernel.Initialize();
        }

        /// <summary>
        /// Called on OnUpdate
        /// </summary>
        public static void Update()
        {
            RapidList<KernelUnit> all = Kernel.Units;
            for (int i = 0; i < all.Count; ++i)
            {
                KernelUnit kernelUnit = all[i];
                if (kernelUnit == null)
                {
                    Log.InternalError("engine caught unexpected error while update : kernel unit is null");
                    continue;
                }

                try
                {
                    kernelUnit.Update(Time.deltaTime);
                }
                catch (Exception e)
                {
                    Log.InternalException(e);
                }
            }
        }

        /// <summary>
        /// Called on LateUpdate
        /// </summary>
        public static void LateUpdate()
        {
            RapidList<KernelUnit> all = Kernel.Units;
            for (int i = 0; i < all.Count; ++i)
            {
                KernelUnit kernelUnit = all[i];
                if (kernelUnit == null)
                {
                    Log.InternalError("engine caught unexpected error while update : kernel unit is null");
                    continue;
                }

                try
                {
                    kernelUnit.LateUpdate();
                }
                catch (Exception e)
                {
                    Log.InternalException(e);
                }
            }

            VarList.AutoRelease();
        }

        /// <summary>
        /// Called on FixedUpdate
        /// </summary>
        public static void FixedUpdate()
        {
            RapidList<KernelUnit> all = Kernel.Units;
            for (int i = 0; i < all.Count; ++i)
            {
                KernelUnit kernelUnit = all[i];
                if (kernelUnit == null)
                {
                    Log.InternalError("engine caught unexpected error while update : kernel unit is null");
                    continue;
                }

                try
                {
                    kernelUnit.FixedUpdate();
                }
                catch (Exception e)
                {
                    Log.InternalException(e);
                }
            }
        }

        /// <summary>
        /// Called on ResetToCache
        /// </summary>
        public static void Reset()
        {
            RapidList<KernelUnit> all = Kernel.Units;
            for (int i = 0; i < all.Count; ++i)
            {
                int index = i;
                Function.Run(() =>
                {
                    KernelUnit kernelUnit = all[index];
                    if (kernelUnit == null)
                    {
                        Log.InternalError("engine caught unexpected error while update : kernel unit is null");
                        return;
                    }

                    kernelUnit.Reset();
                });
            }
        }

        /// <summary>
        /// Called on Destroy
        /// </summary>
        public static void Destroy()
        {
            RapidList<KernelUnit> all = Kernel.Units;
            for (int i = 0; i < all.Count; ++i)
            {
                int index = i;
                Function.Run(() =>
                {
                    KernelUnit kernelUnit = all[index];
                    if (kernelUnit == null)
                    {
                        Log.InternalError("engine caught unexpected error while update : kernel unit is null");
                        return;
                    }

                    kernelUnit.Destroy();
                });
            }
        }

        /// <summary>
        /// 游戏退出
        /// </summary>
        public static void Quit()
        {
            RapidList<KernelUnit> all = Kernel.Units;
            for (int i = 0; i < all.Count; ++i)
            {
                int index = i;
                Function.Run(() =>
                {
                    KernelUnit kernelUnit = all[index];
                    if (kernelUnit == null)
                    {
                        Log.InternalError("engine caught unexpected error while update : kernel unit is null");
                        return;
                    }

                    kernelUnit.Quit();
                });
            }
        }

        /// <summary>
        /// 游戏暂停
        /// </summary>
        /// <param name="isPause"></param>
        public static void Pause(bool isPause)
        {
            RapidList<KernelUnit> all = Kernel.Units;
            for (int i = 0; i < all.Count; ++i)
            {
                int index = i;
                Function.Run(() =>
                {
                    KernelUnit kernelUnit = all[index];
                    if (kernelUnit == null)
                    {
                        Log.InternalError("engine caught unexpected error while update : kernel unit is null");
                        return;
                    }

                    kernelUnit.Pause(isPause);
                });
            }
        }
        
        #endregion

        #region Launch

        /// <summary>
        /// 获取逻辑模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetModule<T>() where T : CloverModule
        {
            CloverModuleSet moduleSet = Kernel.Get<CloverModuleSet>();
            if (moduleSet == null)
            {
                Log.UnexpectedError(nameof(RegisterModules));
                return default;
            }

            return moduleSet.GetModule<T>();
        }

        /// <summary>
        /// 启动时注册所有业务逻辑模块
        /// </summary>
        /// <param name="modules"></param>
        public static void RegisterModules(CloverModule[] modules)
        {
            if (modules == null)
            {
                return;
            }

            CloverModuleSet moduleSet = Kernel.Get<CloverModuleSet>();
            if (moduleSet == null)
            {
                Log.UnexpectedError(nameof(RegisterModules));
                return;
            }

            moduleSet.AddModules(modules);
        }

        #endregion

        #region RunTime

        /// <summary>
        /// 开启协程
        /// </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            return CloverActorHierarchy.Engine.StartCoroutine(routine);
        }

        /// <summary>
        /// 订阅类事件
        /// </summary>
        /// <param name="className"></param>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public static void AddClassEvent(string className, EClassEvent eventType, LogicEventCallBack callBack)
        {
            if (string.IsNullOrEmpty(className))
            {
                Log.InternalError($"class name can not be null while {nameof(AddClassEvent)}");
                return;
            }

            if (callBack == null)
            {
                Log.InternalError("callback is null");
                return;
            }

            CloverClassEventLibrary eventLibrary = Kernel.Get<CloverClassEventLibrary>();
            if (eventLibrary == default)
            {
                Log.UnexpectedError(nameof(AddClassEvent));
                return;
            }

            ClassEventContext context = eventLibrary.Get(className);
            if (context == CloverClassEventLibrary.Default)
            {
                return;
            }

            context.RegisterClassEvent(eventType, callBack);
        }

        #endregion
    }
}