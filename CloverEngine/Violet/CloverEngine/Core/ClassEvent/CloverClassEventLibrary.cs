﻿using System.Collections.Generic;

namespace Clover
{
    public enum EClassEvent
    {
        OnCreate,
        OnDestroy,
        OnDataReady,
        OnAddChild,
        OnAddChildren,
        OnEntryOther,
        OnCapacityChange,

        OnLeaveOther,
        OnRemoveChild,
        Max,
    }

    /// <summary>
    /// 类事件中心
    /// </summary>
    internal class CloverClassEventLibrary : KernelUnit
    {
        readonly Dictionary<string, ClassEventContext> m_ClassContexts = new Dictionary<string, ClassEventContext>();

        public static readonly ClassEventContext Default = ClassEventContext.Create();

        public ClassEventContext Get(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                Log.InternalError($"class name is null or empty while find class event context");
                return Default;
            }

            if (m_ClassContexts.TryGetValue(className, out ClassEventContext context))
            {
                return context;
            }

            context = Alloc(className);
            return context;
        }

        public ClassEventContext Alloc(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                Log.InternalError($"class name is null or empty while alloc class event context");
                return Default;
            }

            if (m_ClassContexts.TryGetValue(className, out ClassEventContext context))
            {
                return context;
            }

            context = ClassEventContext.Create();
            m_ClassContexts[className] = context;
            return context;
        }

        public void Release(ClassEventContext context)
        {
            if (context == null)
            {
                Log.InternalError($"context is destroyed somewhere, it shouldn't happen , plz check this out");
                return;
            }

            context.Reset();
        }
    }
}