﻿﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Clover
{
    internal class CloverModuleSet : KernelUnit
    {
        readonly List<CloverModule> m_Modules = new List<CloverModule>();
        readonly List<CloverModule> m_UpdateModules = new List<CloverModule>();
        readonly List<CloverModule> m_LateUpdateModules = new List<CloverModule>();

        /// <summary>
        /// 去重集合
        /// </summary>
        readonly HashSet<CloverModule> m_Set = new HashSet<CloverModule>();

        /// <summary>
        /// 类型映射
        /// </summary>
        readonly Dictionary<Type, CloverModule> m_TypeDict = new Dictionary<Type, CloverModule>();

        public void AddModules(CloverModule[] modules)
        {
            if (modules == null)
            {
                Log.InternalError("modules are null while init");
                return;
            }

            Helper.Foreach(modules, AddModule);
            Helper.Foreach(modules, module => { Function.Run(module.Init); });
        }

        public void AddModule(CloverModule module)
        {
            if (module == null)
            {
                return;
            }

            if (Contains(module))
            {
                return;
            }

            //注册实例
            //Add World
            m_Modules.Add(module);
            m_Set.Add(module);

            //Type Mapping
            Type type = module.GetType();
            m_TypeDict[type] = module;

            //将覆盖过的OnUpdate加入列表
            MethodInfo updateMethod = type.GetMethod("OnUpdate", (BindingFlags) (-1));
            if (updateMethod != null && updateMethod != updateMethod.GetBaseDefinition())
            {
                m_UpdateModules.Add(module);
            }

            //将覆盖过的OnLateUpdate加入列表
            MethodInfo lateUpdateMethod = type.GetMethod("OnLateUpdate", (BindingFlags) (-1));
            if (lateUpdateMethod != null && lateUpdateMethod != lateUpdateMethod.GetBaseDefinition())
            {
                m_LateUpdateModules.Add(module);
            }
        }

        public bool Contains(CloverModule module)
        {
            return module != null && m_Set.Contains(module);
        }

        public T GetModule<T>() where T : CloverModule
        {
            if (m_TypeDict.TryGetValue(typeof(T), out CloverModule ret))
            {
                return ret as T;
            }

            return null;
        }

        public CloverModule GetModule(Type moduleType)
        {
            if (moduleType == null)
            {
                return default;
            }

            return m_TypeDict.TryGetValue(moduleType, out CloverModule ret) ? ret : default;
        }

        public override void Update(float deltaTime)
        {
            for (int i = 0; i < m_UpdateModules.Count; i++)
            {
                CloverModule module = m_UpdateModules[i];
                module.Update();
            }
        }

        public override void LateUpdate()
        {
            for (int i = 0; i < m_LateUpdateModules.Count; i++)
            {
                CloverModule module = m_LateUpdateModules[i];
                module.LateUpdate();
            }
        }

        public override void Destroy()
        {
            for (int i = 0; i < m_Modules.Count; i++)
            {
                CloverModule module = m_Modules[i];
                module.ShutDown();
            }
        }

        public override void Reset()
        {
            for (int i = 0; i < m_Modules.Count; i++)
            {
                CloverModule module = m_Modules[i];
                module.Reset();
            }
        }

        public override void Dump(VarList result)
        {
            for (int i = 0; i < m_Modules.Count; i++)
            {
                CloverModule module = m_Modules[i];
                result = result < "[Module]" < module.name;
            }
        }

        protected override void Background()
        {
            for (int i = 0; i < m_Modules.Count; i++)
            {
                CloverModule module = m_Modules[i];
                module.Background();
            }
        }

        protected override void Foreground()
        {
            for (int i = 0; i < m_Modules.Count; i++)
            {
                CloverModule module = m_Modules[i];
                module.Foreground();
            }
        }
    }
}