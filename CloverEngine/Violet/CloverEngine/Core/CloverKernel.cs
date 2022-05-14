using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clover
{
    internal class CloverKernel
    {
        readonly Dictionary<Type, int> m_TypeMapping = new Dictionary<Type, int>();

        /// <summary>
        /// 提前构造模块, 无依赖任何模块
        /// </summary>
        static readonly KernelUnit[] PreConstructUnits =
        {
            KernelUnit.Create<CloverFunctionLibrary>(),
            KernelUnit.Create<CloverHeartBeatLibrary>(),
            KernelUnit.Create<CloverPropertyLibrary>(),
            KernelUnit.Create<CloverClassEventLibrary>(),
            KernelUnit.Create<CloverWorkerLibrary>(),
        };

        /// <summary>
        /// 可能依赖预购造模块
        /// </summary>
        static readonly KernelUnit[] ConstructUnits =
        {
            KernelUnit.Create<CloverActorHierarchy>(),
        };

        /// <summary>
        /// 业务模块, 可能依赖预购造和构造模块
        /// </summary>
        static readonly KernelUnit[] PostConstructUnits =
        {
            KernelUnit.Create<CloverModuleSet>(),
        };

        Type m_L1CacheType;
        KernelUnit m_L1CacheUnit;

        /// <summary>
        /// AllUnits Units
        /// </summary>
        /// <returns></returns>
        public readonly RapidList<KernelUnit> Units = new RapidList<KernelUnit>();

        /// <summary>
        /// 内核初始化
        /// </summary>
        public void Initialize()
        {
            Log.InternalInfo("Build Kernel Start");

            //PreInitializeUnits
            InitializeUnits(PreConstructUnits);

            //Initialize
            InitializeUnits(ConstructUnits);

            //PostInitialize
            InitializeUnits(PostConstructUnits);

            Log.InternalInfo("Build Kernel End");
        }

        /// <summary>
        /// Register unit
        /// </summary>
        /// <param name="unit"></param>
        void Register(KernelUnit unit)
        {
            if (unit == null)
            {
                return;
            }

            Type type = unit.GetType();
            Units.Add(unit);
            m_TypeMapping[type] = Units.Count - 1;
        }

        /// <summary>
        /// Generic Getter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : KernelUnit
        {
            Type type = typeof(T);
            if (type == m_L1CacheType && m_L1CacheUnit != null)
            {
                return m_L1CacheUnit as T;
            }

            if (!m_TypeMapping.TryGetValue(type, out int index))
            {
                return default;
            }

            KernelUnit unit = Units[index];
            m_L1CacheType = type;
            m_L1CacheUnit = unit;
            return unit as T;
        }

        void InitializeUnits(IReadOnlyList<KernelUnit> units)
        {
            for (int i = 0; i < units.Count; ++i)
            {
                KernelUnit unit = units[i];
                Register(unit);
                unit.Kernel = this;
                unit.OnBuild();
                Application.lowMemory += unit.OnLowMemory;
            }
        }
    }
}