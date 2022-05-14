﻿using System.Collections.Generic;

namespace Clover
{
    public class ClassEventContext
    {
        public ClassEvent[] EventSet = new ClassEvent[(int) EClassEvent.Max];

        public List<ClassEventContext> Children = new List<ClassEventContext>();

        public static ClassEventContext Create()
        {
            ClassEventContext context = new ClassEventContext();
            context.Init();
            return context;
        }

        /// <summary>
        /// 添加子类
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(ClassEventContext child)
        {
            if (child == null)
            {
                return;
            }

            Children.Add(child);
            child.Inherit(this);
        }

        /// <summary>
        /// 添加类事件回调
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventCallBack"></param>
        public void RegisterClassEvent(EClassEvent eventType, LogicEventCallBack eventCallBack)
        {
            if (eventCallBack == null)
            {
                Log.InternalError($"event can not be null while add event to ClassEventContext");
                return;
            }

            int index = (int) eventType;
            if (!Helper.IsValidIndex(index, EventSet.Length))
            {
                Log.InternalError($"event type is invalid : {eventType}, while add class event to ClassEventContext");
                return;
            }

            ClassEvent classEvent = EventSet[index];
            classEvent.Add(eventCallBack);

            for (int i = 0; i < Children.Count; ++i)
            {
                ClassEventContext child = Children[i];
                child.RegisterClassEvent(eventType, eventCallBack);
            }
        }

        /// <summary>
        /// 执行类事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="self"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ExecuteClassEvent(EClassEvent eventType, ActorID self, ActorID sender, VarList args)
        {
            int index = (int) eventType;
            if (!Helper.IsValidIndex(index, EventSet.Length))
            {
                Log.InternalError($"logic event type is not valid {eventType} while execute");
                return;
            }

            ClassEvent classEvent = EventSet[index];
            classEvent.Execute(self, sender, args);
        }

        /// <summary>
        /// 继承父类所有逻辑事件
        /// </summary>
        /// <param name="parent"></param>
        public void Inherit(ClassEventContext parent)
        {
            if (parent == null)
            {
                return;
            }

            for (int i = 0; i < parent.EventSet.Length; i++)
            {
                ClassEvent parentEvent = parent.EventSet[i];
                ClassEvent thisEvent = EventSet[i];
                thisEvent.Add(parentEvent);
            }
        }

        public void Reset()
        {
            foreach (ClassEvent classEvent in EventSet)
            {
                classEvent.Reset();
            }
        }

        void Init()
        {
            for (int i = 0; i < (int) EClassEvent.Max; ++i)
            {
                EventSet[i] = new ClassEvent();
            }
        }
    }
}