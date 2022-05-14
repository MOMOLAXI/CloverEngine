﻿using System;
using System.Collections.Generic;

namespace Clover
{
    /// <summary>
    /// 逻辑事件回调
    /// </summary>
    /// <param name="self"></param>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void LogicEventCallBack(ActorID self, ActorID sender, VarList args);

    public class ClassEvent
    {
        public int Count => Events.Count;

        public List<LogicEventCallBack> Events = new List<LogicEventCallBack>();

        public LogicEventCallBack Get(int index)
        {
            return !Helper.IsValidIndex(index, Events.Count) ? Empty : Events[index];
        }

        public void Reset()
        {
            Events.Clear();
        }

        public void Execute(ActorID self, ActorID sender, VarList args)
        {
            for (int i = 0; i < Events.Count; i++)
            {
                LogicEventCallBack eventCallBack = Events[i];
                if (eventCallBack == null)
                {
                    continue;
                }

                try
                {
                    eventCallBack.Invoke(self, sender, args);
                }
                catch (Exception e)
                {
                    Log.InternalException(e);
                }
            }
        }

        public void Add(ClassEvent other)
        {
            if (other == null)
            {
                return;
            }

            Events.AddRange(other.Events);
        }

        public void Add(LogicEventCallBack eventCallBack)
        {
            if (eventCallBack == null)
            {
                return;
            }

            Events.Add(eventCallBack);
        }

        public void Remove(int index)
        {
            if (!Helper.IsValidIndex(index, Events.Count))
            {
                return;
            }

            Events.RemoveAt(index);
        }

        static void Empty(ActorID self, ActorID sender, VarList args) { }
    }
}