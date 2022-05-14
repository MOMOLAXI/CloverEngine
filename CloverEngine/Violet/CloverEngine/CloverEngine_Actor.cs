﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clover
{
    public static partial class CloverEngine
    {
        /// <summary>
        /// 获取当前场景
        /// </summary>
        /// <returns></returns>
        public static ActorID GetCurrentScene()
        {
            return Kernel.Get<CloverActorHierarchy>().CurSceneID;
        }

        /// <summary>
        /// 创建场景
        /// </summary>
        /// <returns></returns>
        public static ActorID CreateScene()
        {
            CloverActorHierarchy hierarchy = Kernel.Get<CloverActorHierarchy>();
            if (hierarchy == null)
            {
                Log.UnexpectedError(nameof(CreateScene));
                return ActorID.zero;
            }

            return hierarchy.CreateScene();
        }

        /// <summary>
        /// 创建Actor
        /// </summary>
        /// <param name="className"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public static ActorID CreateActor(string className, ActorID parentID)
        {
            if (parentID == ActorID.zero)
            {
                Log.InternalError($"actor id is not valid");
                return ActorID.zero;
            }

            CloverActorHierarchy hierarchy = Kernel.Get<CloverActorHierarchy>();
            if (hierarchy == null)
            {
                Log.UnexpectedError(nameof(CreateActor));
                return ActorID.zero;
            }

            Actor parent = hierarchy.Find(parentID);
            Actor actor = hierarchy.CreateActor(className, parent);
            return actor.ID;
        }

        /// <summary>
        /// 创建Actor
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static ActorID CreateActor(string className)
        {
            CloverActorHierarchy hierarchy = Kernel.Get<CloverActorHierarchy>();
            if (hierarchy == null)
            {
                Log.UnexpectedError(nameof(CreateActor));
                return ActorID.zero;
            }

            Actor actor = hierarchy.CreateActor(className, CloverActorHierarchy.Engine);
            return actor.ID;
        }

        /// <summary>
        /// 删除Actor
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="immediate"></param>
        public static void DestroyActor(ActorID actorId, bool immediate = false)
        {
            if (actorId == ActorID.zero)
            {
                Log.InternalError($"actor id is not valid");
                return;
            }

            CloverActorHierarchy hierarchy = Kernel.Get<CloverActorHierarchy>();
            if (hierarchy == null)
            {
                Log.UnexpectedError(nameof(DestroyActor));
                return;
            }

            hierarchy.DestroyActor(actorId, immediate);
        }

        /// <summary>
        /// Actor添加Mono组件
        /// </summary>
        /// <param name="actorId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T AddComponent<T>(ActorID actorId) where T : Component
        {
            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.AddComponent<T>();
        }

        /// <summary>
        /// Actor添加逻辑组件
        /// </summary>
        /// <param name="actorId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T AddActorComponent<T>(ActorID actorId) where T : ActorComponent, new()
        {
            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.AddActorComponent<T>();
        }

        /// <summary>
        /// Actor添加逻辑组件
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ActorComponent AddActorComponent(ActorID actorId, Type type)
        {
            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.AddActorComponent(type);
        }

        /// <summary>
        /// 获取逻辑组件
        /// </summary>
        /// <param name="actorId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetActorComponent<T>(ActorID actorId) where T : ActorComponent
        {
            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.GetActorComponent<T>();
        }

        /// <summary>
        /// 获取Mono组件
        /// </summary>
        /// <param name="actorId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetComponent<T>(ActorID actorId) where T : Component
        {
            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.GetComponent<T>();
        }

        /// <summary>
        /// 移除Actor逻辑组件
        /// </summary>
        /// <param name="actorId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool RemoveActorComponent<T>(ActorID actorId) where T : ActorComponent
        {
            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return false;
            }

            return actor.RemoveActorComponent<T>();
        }

        /// <summary>
        /// 移除Actor逻辑组件
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool RemoveActorComponent(ActorID actorId, Type type)
        {
            if (type == null)
            {
                Log.InternalError($"can not remove null type");
                return false;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return false;
            }

            return actor.RemoveActorComponent(type);
        }
        
        /// <summary>
        /// 移除Actor逻辑组件
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool RemoveActorComponent(ActorID actorId, ActorComponent component)
        {
            if (component == null)
            {
                Log.InternalError($"can not remove null type");
                return false;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return false;
            }

            return actor.RemoveActorComponent(component);
        }

        /// <summary>
        /// 获取Actor对应Transform
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        public static Transform GetTransform(ActorID actorId)
        {
            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.transform;
        }

        /// <summary>
        /// 索引获取子Actor
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static ActorID GetChildAtIndex(ActorID actorId, int index)
        {
            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                return default;
            }

            return actor.GetChildAtIndex(index);
        }

        /// <summary>
        /// 获取子对象
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="result"></param>
        public static void GetChildren(ActorID actorId, List<ActorID> result)
        {
            if (result == null)
            {
                Log.InternalError("input list is null");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.GetChildren(result);
        }

        /// <summary>
        /// 获取子对象
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="result"></param>
        public static void GetChildren(ActorID actorId, VarList result)
        {
            if (result == null)
            {
                Log.InternalError("input list is null");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.GetChildren(result);
        }

        /// <summary>
        /// 添加属性变化回调
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="callbackName"></param>
        /// <param name="function"></param>
        public static void AddPropHook(
            ActorID actorId,
            string propName,
            string callbackName,
            PropChangeFunction function)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("[AddPropHook]property name must not be null or empty");
                return;
            }

            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError("[AddPropHook]callback name must not be null or empty");
                return;
            }

            if (function == null)
            {
                Log.InternalError("[AddPropHook] function is null");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.AddPropHook(propName, callbackName, function);
        }

        /// <summary>
        /// 移除属性变化回调
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="callbackName"></param>
        public static void RemovePropHook(ActorID actorId, string propName, string callbackName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("[RemovePropHook]property name must not be null or empty");
                return;
            }

            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError("[RemovePropHook]callback name must not be null or empty");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.RemovePropHook(propName, callbackName);
        }

        /// <summary>
        /// 获取Actor的bool属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static bool GetBool(ActorID actorId, string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.GetBool(propName);
        }

        /// <summary>
        /// 获取Actor的Int属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static int GetInt(ActorID actorId, string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.GetInt(propName);
        }

        /// <summary>
        /// 获取Actor的long属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static long GetInt64(ActorID actorId, string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.GetInt64(propName);
        }

        /// <summary>
        /// 获取Actor的float属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static float GetFloat(ActorID actorId, string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.GetFloat(propName);
        }

        /// <summary>
        /// 获取Actor的double属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static double GetDouble(ActorID actorId, string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.GetDouble(propName);
        }

        /// <summary>
        /// 获取角色身上的字符串属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static string GetString(ActorID actorId, string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.GetString(propName);
        }

        /// <summary>
        /// 获取Actor的ActorID属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static ActorID GetActorID(ActorID actorId, string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return ActorID.zero;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return ActorID.zero;
            }

            return actor.GetActorID(propName);
        }

        /// <summary>
        /// 获取Actor的二进制属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static byte[] GetBinary(ActorID actorId, string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return EmptyArray<byte>.Value;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return EmptyArray<byte>.Value;
            }

            return actor.GetBinary(propName);
        }

        /// <summary>
        /// 获取Actor的对象属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object GetObject(ActorID actorId, string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.GetObject(propName);
        }

        /// <summary>
        /// 获取Actor的对象属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static T GetClass<T>(ActorID actorId, string propName) where T : class
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return default;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor.GetClass<T>(propName);
        }

        /// <summary>
        /// 设置Actor的bool属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetBool(ActorID actorId, string propName, bool value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.SetBool(propName, value);
        }

        /// <summary>
        /// 设置Actor的int属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetInt(ActorID actorId, string propName, int value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.SetInt(propName, value);
        }

        /// <summary>
        /// 设置Actor的long属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetInt64(ActorID actorId, string propName, long value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.SetInt64(propName, value);
        }

        /// <summary>
        /// 设置Actor的float属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetFloat(ActorID actorId, string propName, float value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.SetFloat(propName, value);
        }

        /// <summary>
        /// 设置Actor的double属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetDouble(ActorID actorId, string propName, double value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.SetDouble(propName, value);
        }

        /// <summary>
        /// 设置Actor的string属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetString(ActorID actorId, string propName, string value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.SetString(propName, value);
        }

        /// <summary>
        /// 设置Actor的ActorID属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetActorID(ActorID actorId, string propName, ActorID value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.SetActorID(propName, value);
        }

        /// <summary>
        /// 设置Actor的二进制属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetBinary(ActorID actorId, string propName, byte[] value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.SetBinary(propName, value);
        }

        /// <summary>
        /// 设置Actor的对象属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetObject(ActorID actorId, string propName, object value)
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.SetObject(propName, value);
        }

        /// <summary>
        /// 设置Actor的对象属性
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetClass<T>(ActorID actorId, string propName, T value) where T : class
        {
            if (string.IsNullOrEmpty(propName))
            {
                Log.InternalError("property name must not be null or empty");
                return;
            }

            Actor actor = GetActor(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return;
            }

            actor.SetClass(propName, value);
        }

        /// <summary>
        /// 获取Actor
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        internal static Actor GetActor(ActorID actorId)
        {
            if (actorId == ActorID.zero)
            {
                Log.InternalError($"actor id is not valid");
                return default;
            }

            CloverActorHierarchy hierarchy = Kernel.Get<CloverActorHierarchy>();
            if (hierarchy == null)
            {
                Log.UnexpectedError(nameof(GetActorComponent));
                return default;
            }

            Actor actor = hierarchy.Find(actorId);
            if (actor == null)
            {
                Log.InternalError($"actor not exist with ID {actorId}");
                return default;
            }

            return actor;
        }
    }
}