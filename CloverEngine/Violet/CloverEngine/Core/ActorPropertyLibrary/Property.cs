﻿using System;

namespace Clover
{
    /// <summary>
    /// 属性变化回调
    /// </summary>
    /// <param name="self"></param>
    /// <param name="property"></param>
    /// <param name="last"></param>
    /// <param name="cur"></param>
    public delegate void PropChangeFunction(ActorID self, string property, Var last, Var cur);

    [Serializable]
    public class CloverActorProperty : ICacheAble
    {
        Var m_Value;

        public string Name { get; set; }

        public Var Value
        {
            get => m_Value;
            set
            {
                Var last = m_Value;
                m_Value = value;
                for (int i = 0; i < PropListener.All.Count; i++)
                {
                    PropChangeFunction function = PropListener.All[i];
                    function?.Invoke(Owner, Name, last, value);
                }
            }
        }

        public ActorID Owner { get; set; }

        FastCollection<string, PropChangeFunction> m_PropListener;

        FastCollection<string, PropChangeFunction> PropListener =>
            m_PropListener ??= new FastCollection<string, PropChangeFunction>();

        public void AddPropHook(string callbackName, PropChangeFunction function)
        {
            if (string.IsNullOrEmpty(callbackName) || function == null)
            {
                return;
            }

            PropListener.Add(callbackName, function);
        }

        public void RemovePropHook(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                return;
            }

            m_PropListener?.Remove(callbackName);
        }

        public void Reset()
        {
            Name = string.Empty;
            Value = Var.zero;
            Owner = ActorID.zero;
            m_PropListener?.Clear();
        }

        public bool cacheFlag { get; set; }
    }
}