﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Clover
{
    public partial class Actor : MonoBehaviour
    {
        bool m_IsDestroying;
        bool m_NeedUpdateComponent;
        readonly RapidList<ActorComponent> m_Components = new RapidList<ActorComponent>();
        static readonly PolyObjectPool<ActorComponent> s_ComponentPool = new PolyObjectPool<ActorComponent>();

        /// <summary>
        /// 对象ID
        /// </summary>
        public ActorID ID { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 父类
        /// </summary>
        public Actor Parent { get; set; }

        /// <summary>
        /// 销毁时通知
        /// </summary>
        public Run<Actor> OnDestroyActor { get; set; }

        /// <summary>
        /// 子类
        /// </summary>
        public List<Actor> Children { get; } = new List<Actor>();

        /// <summary>
        /// 类事件触发器 
        /// </summary>
        public ClassEventContext EventContext { get; private set; }

        /// <summary>
        /// 属性
        /// </summary>
        public PropertyCollection Properties;

        /// <summary>
        /// 订阅属性变化
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="callbackName"></param>
        /// <param name="function"></param>
        public void AddPropHook(string propName, string callbackName, PropChangeFunction function)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SubscribePropChange]property name not exist : {propName}");
                return;
            }

            property.AddPropHook(callbackName, function);
        }

        /// <summary>
        /// 移除属性变化监听
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="callbackName"></param>
        public void RemovePropHook(string propName, string callbackName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SubscribePropChange]property name not exist : {propName}");
                return;
            }

            property.RemovePropHook(callbackName);
        }

        public void SetBool(string propName, bool value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SetBool]property name not exist : {propName}");
                return;
            }

            property.Value = new Var(value);
        }

        public void SetInt(string propName, int value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SetInt]property name not exist : {propName}");
                return;
            }

            property.Value = new Var(value);
        }

        public void SetInt64(string propName, long value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SetInt64]property name not exist : {propName}");
                return;
            }

            property.Value = new Var(value);
        }

        public void SetFloat(string propName, float value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SetFloat]property name not exist : {propName}");
                return;
            }

            property.Value = new Var(value);
        }

        public void SetDouble(string propName, double value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SetDouble]property name not exist : {propName}");
                return;
            }

            property.Value = new Var(value);
        }

        public void SetString(string propName, string value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SetString]property name not exist : {propName}");
                return;
            }

            property.Value = new Var(value);
        }

        public void SetActorID(string propName, ActorID value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SetActorID]property name not exist : {propName}");
                return;
            }

            property.Value = new Var(value);
        }

        public void SetObject(string propName, object value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SetObject]property name not exist : {propName}");
                return;
            }

            property.Value = new Var(value);
        }

        public void SetClass<T>(string propName, T value) where T : class
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SetClass<T>]property name not exist : {propName}");
                return;
            }

            property.Value = new Var(value);
        }

        public void SetBinary(string propName, byte[] value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::SetBinary]property name not exist : {propName}");
                return;
            }

            property.Value = new Var(value);
        }

        public bool GetBool(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::GetBool]property name not exist : {propName}");
                return default;
            }

            return property.Value.GetBool();
        }

        public int GetInt(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::GetInt]property name not exist : {propName}");
                return default;
            }

            return property.Value.GetInt();
        }

        public long GetInt64(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::GetInt64]property name not exist : {propName}");
                return default;
            }

            return property.Value.GetInt64();
        }

        public float GetFloat(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::GetFloat]property name not exist : {propName}");
                return default;
            }

            return property.Value.GetFloat();
        }

        public double GetDouble(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::GetDouble]property name not exist : {propName}");
                return default;
            }

            return property.Value.GetDouble();
        }

        public string GetString(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::GetString]property name not exist : {propName}");
                return default;
            }

            return property.Value.GetString();
        }

        public ActorID GetActorID(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return ActorID.zero;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::GetActorID]property name not exist : {propName}");
                return ActorID.zero;
            }

            return property.Value.GetActorID();
        }

        public byte[] GetBinary(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::GetBinary]property name not exist : {propName}");
                return default;
            }

            return property.Value.GetBinary();
        }

        public object GetObject(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::GetObject]property name not exist : {propName}");
                return default;
            }

            return property.Value.GetObject();
        }

        public T GetClass<T>(string propName) where T : class
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            CloverActorProperty property = Properties.Get(propName);
            if (property == Empty<CloverActorProperty>.Value)
            {
                Log.InternalError($"[Actor::GetClass]property name not exist : {propName}");
                return default;
            }

            return property.Value.GetObject() as T;
        }

        /// <summary>
        /// 发送类消息
        /// </summary>
        /// <param name="logicEvent"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal void SendClassEvent(EClassEvent logicEvent, ActorID sender, VarList args)
        {
            if (EventContext == null)
            {
                Log.InternalError($"{ClassName} parent's event context is missing");
                return;
            }

            EventContext.ExecuteClassEvent(logicEvent, ID, sender, args);
        }

        /// <summary>
        /// 初始化属性集合
        /// </summary>
        /// <param name="collection"></param>
        internal void InitializeProperty(PropertyCollection collection)
        {
            if (collection == null)
            {
                Log.InternalError($"{ClassName} parent's property context is missing");
                return;
            }

            Properties = collection;
            foreach (KeyValuePair<string, CloverActorProperty> property in Properties.Properties)
            {
                property.Value.Owner = ID;
            }
        }

        /// <summary>
        /// 设置类消息上下文
        /// </summary>
        /// <param name="eventContext"></param>
        internal void SetEventContext(ClassEventContext eventContext)
        {
            if (eventContext == null)
            {
                Log.InternalError($"{ClassName} parent's event context is missing");
                return;
            }

            EventContext = eventContext;
        }

        /// <summary>
        /// 继承父类事件
        /// </summary>
        /// <param name="parentContext"></param>
        internal void Inherit(ClassEventContext parentContext)
        {
            if (parentContext == null)
            {
                Log.InternalError($"{ClassName} inherit error, parent context is null, check parent alloc");
                return;
            }

            EventContext?.Inherit(parentContext);
        }

        /// <summary>
        /// 添加子对象
        /// </summary>
        /// <param name="actors"></param>
        internal void AddChildren(List<Actor> actors)
        {
            if (actors == null)
            {
                Log.InternalError($"actor list is null while add children to {ClassName}");
                return;
            }

            foreach (Actor actor in actors)
            {
                actor.Parent = this;
                actor.SendClassEvent(EClassEvent.OnEntryOther, ID, VarList.Empty);
            }

            Children.AddRange(actors);
            SendClassEvent(EClassEvent.OnAddChildren, ID, VarList.Empty);
            SendClassEvent(EClassEvent.OnCapacityChange, ID, VarList.Empty);
        }

        /// <summary>
        /// 添加子对象
        /// </summary>
        /// <param name="actor"></param>
        internal void AddChild(Actor actor)
        {
            if (actor == null)
            {
                Log.InternalError($"actor is null while add child to {ClassName}");
                return;
            }

            actor.Parent = this;
            actor.Inherit(EventContext);
            Children.Add(actor);
            SendClassEvent(EClassEvent.OnAddChild, actor.ID, VarList.Empty);
            SendClassEvent(EClassEvent.OnCapacityChange, actor.ID, VarList.Empty);
            actor.SendClassEvent(EClassEvent.OnEntryOther, ID, VarList.Empty);
        }

        /// <summary>
        /// 移除子对象
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="destroy"></param>
        internal void RemoveChild(Actor actor, bool destroy = false)
        {
            if (actor == null)
            {
                return;
            }

            Children.Remove(actor);

            SendClassEvent(EClassEvent.OnRemoveChild, actor.ID, VarList.Empty);
            SendClassEvent(EClassEvent.OnCapacityChange, actor.ID, VarList.Empty);
            actor.SendClassEvent(EClassEvent.OnLeaveOther, ID, VarList.Empty);

            if (destroy)
            {
                OnDestroyActor?.Invoke(actor);
            }
        }

        /// <summary>
        /// 通过索引获取子对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ActorID GetChildAtIndex(int index)
        {
            return Helper.IsValidIndex(index, Children.Count) ? Children[index].ID : ActorID.zero;
        }

        /// <summary>
        /// 获取所有子对象
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public int GetChildren(VarList result)
        {
            result.Clear();

            for (int i = 0; i < Children.Count; ++i)
            {
                Actor child = Children[i];
                result.AddObject(child.ID);
            }

            return result.Count;
        }

        /// <summary>
        /// 获取所有子对象
        /// </summary>
        /// <param name="children"></param>
        public void GetChildren(List<ActorID> children)
        {
            children?.Clear();
            children?.AddRange(Children.Select(child => child.ID));
        }

        public void ClearChildren(bool destroy)
        {
            if (destroy)
            {
                foreach (Actor child in Children)
                {
                    OnDestroyActor?.Invoke(child);
                }
            }

            Children.Clear();
            SendClassEvent(EClassEvent.OnCapacityChange, ID, VarList.Empty);
        }

        public void OnDestroySelf()
        {
            if (m_IsDestroying)
            {
                Log.InternalError("[Actor:OnDestroySelf]object is in destroying.");
                return;
            }

            m_IsDestroying = true;
            // 清理组件
            RemoveAllComponent();
            ClearChildren(true);
            OnDestroyActor?.Invoke(this);
            SendClassEvent(EClassEvent.OnDestroy, ID, VarList.Empty);
        }

        internal void OnUpdate()
        {
            if (m_NeedUpdateComponent)
            {
                UpdateComponent();
            }
        }

        internal void OnLateUpdate()
        {
            if (m_IsDestroying)
            {
                return;
            }

            if (m_NeedUpdateComponent)
            {
                LateUpdateComponent();
            }
        }

        public T AddActorComponent<T>(bool findExisted = true) where T : ActorComponent, new()
        {
            T component = null;

            if (findExisted)
            {
                component = GetActorComponent<T>();

                if (component != null)
                {
                    return component;
                }
            }

            component = s_ComponentPool.Get<T>();
            component.Init(ID);

            m_Components.Add(component);

            try
            {
                component.Awake();
            }
            catch (Exception ex)
            {
                Log.InternalException(ex);
            }

            m_NeedUpdateComponent = true;

            return component;
        }

        public ActorComponent AddActorComponent(Type type, bool findExisted = true)
        {
            ActorComponent actorComponent = null;

            if (findExisted)
            {
                actorComponent = GetActorComponent(type);

                if (actorComponent != null)
                {
                    return actorComponent;
                }
            }

            actorComponent = s_ComponentPool.Get(type);
            actorComponent.Init(ID);

            m_Components.Add(actorComponent);

            try
            {
                actorComponent.Awake();
            }
            catch (Exception ex)
            {
                Log.InternalException(ex);
            }

            m_NeedUpdateComponent = true;

            return actorComponent;
        }

        public T AddComponent<T>() where T : Component
        {
            return gameObject.AddComponent<T>();
        }

        public T GetActorComponent<T>() where T : ActorComponent
        {
            for (int i = 0; i < m_Components.Count; ++i)
            {
                if (m_Components[i] != null &&
                    m_Components[i].isDestroyed == false &&
                    m_Components[i] is T)
                {
                    return m_Components[i] as T;
                }
            }

            return null;
        }

        public ActorComponent GetActorComponent(Type type)
        {
            for (int i = 0; i < m_Components.Count; ++i)
            {
                if (m_Components[i] != null &&
                    m_Components[i].isDestroyed == false &&
                    m_Components[i].GetType() == type)
                {
                    return m_Components[i];
                }
            }

            return null;
        }

        public bool RemoveActorComponent<T>() where T : ActorComponent
        {
            for (int i = 0; i < m_Components.Count; ++i)
            {
                ActorComponent component = m_Components[i];
                if (!(component is T))
                {
                    continue;
                }

                component.DestroySelf();
                return true;
            }

            return false;
        }

        public bool RemoveActorComponent(Type type)
        {
            for (int i = 0; i < m_Components.Count; ++i)
            {
                ActorComponent component = m_Components[i];
                if (component == null || component.GetType() != type)
                {
                    continue;
                }

                component.DestroySelf();
                return true;
            }

            return false;
        }

        public bool RemoveActorComponent(ActorComponent actorComponent)
        {
            for (int i = 0; i < m_Components.Count; ++i)
            {
                if (m_Components[i] != actorComponent)
                {
                    continue;
                }

                actorComponent.DestroySelf();
                return true;
            }

            return false;
        }

        void RemoveAllComponent()
        {
            for (int i = 0; i < m_Components.Count; ++i)
            {
                if (m_Components[i] != null)
                {
                    m_Components[i].DestroySelf();
                }
            }
        }

        void UpdateComponent()
        {
            bool hasDestroyed = false;

            ActorComponent[] buffer = m_Components.Buffer;
            int count = m_Components.Count;

            for (int i = 0; i < count; ++i)
            {
                ActorComponent actorComponent = buffer[i];

                if (actorComponent.isDestroyed)
                {
                    hasDestroyed = true;
                    continue;
                }

                if (!actorComponent.isActive)
                {
                    continue;
                }

                if (!actorComponent.isStarted)
                {
                    actorComponent.isStarted = true;

                    try
                    {
                        actorComponent.Start();
                    }
                    catch (Exception ex)
                    {
                        Log.InternalException(ex);
                    }

                    if (actorComponent.isDestroyed || !actorComponent.isActive)
                    {
                        continue;
                    }
                }

                try
                {
                    actorComponent.Update();
                }
                catch (Exception ex)
                {
                    Log.InternalException(ex);
                }
            }

            if (!hasDestroyed)
            {
                return;
            }

            for (int i = m_Components.Count - 1; i >= 0; --i)
            {
                ActorComponent component = m_Components[i];
                if (!component.isDestroyed)
                {
                    continue;
                }

                s_ComponentPool.Release(component);
                m_Components.RemoveAt(i);
            }
        }

        void LateUpdateComponent()
        {
            ActorComponent[] buffer = m_Components.Buffer;
            int count = m_Components.Count;

            for (int i = 0; i < count; ++i)
            {
                ActorComponent actorComponent = buffer[i];

                if (actorComponent.isDestroyed || !actorComponent.isActive)
                {
                    continue;
                }

                try
                {
                    actorComponent.LateUpdate();
                }
                catch (Exception ex)
                {
                    Log.InternalException(ex);
                }
            }
        }
    }
}