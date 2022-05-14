﻿using System.Collections.Generic;
using UnityEngine;

namespace Clover
{
    public delegate void GlobalHeartBeatFunction(float duration);

    public delegate void GlobalCountBeatFunction(int count);

    public enum EHearBeatState
    {
        Idle,
        Beating,
    }

    public class CountBeat : ICacheAble
    {
        /// <summary>
        /// 心跳名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 心跳计数函数
        /// </summary>
        public GlobalCountBeatFunction Function { get; set; }

        /// <summary>
        /// 记录开始的时间点
        /// </summary>
        public float LastTime { get; set; } = -1;

        /// <summary>
        /// 心跳次数
        /// </summary>
        public int BeatCount { get; set; } = -1;

        /// <summary>
        /// 当前计数
        /// </summary>
        public int CurCount { get; set; }

        /// <summary>
        /// 心跳间隔
        /// </summary>
        public float Interval { get; set; }

        /// <summary>
        /// 已经走过的间隔时间
        /// </summary>
        public float IntervalRunningTime { get; set; }

        /// <summary>
        /// 执行状态
        /// </summary>
        public EHearBeatState State { get; set; } = EHearBeatState.Idle;

        public void Invoke()
        {
            Function?.Invoke(CurCount);
        }

        public bool cacheFlag { get; set; }
        public void Reset() { }
    }

    public class HeartBeat : ICacheAble
    {
        /// <summary>
        /// 心跳名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 心跳函数
        /// </summary>
        public GlobalHeartBeatFunction Function { get; set; }

        /// <summary>
        /// 记录开始的时间点
        /// </summary>
        public float LastTime { get; set; } = -1;

        /// <summary>
        /// 运行总时长
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// 心跳间隔
        /// </summary>
        public float Interval { get; set; }

        /// <summary>
        /// 已经走过的间隔时间
        /// </summary>
        public float IntervalRunningTime { get; set; }

        /// <summary>
        /// 执行状态
        /// </summary>
        public EHearBeatState State { get; set; } = EHearBeatState.Idle;

        public void Invoke()
        {
            Function?.Invoke(Time.time - LastTime);
        }

        public bool cacheFlag { get; set; }
        public void Reset() { }
    }

    internal class CloverHeartBeatLibrary : KernelUnit
    {
        readonly FastCollection<string, HeartBeat> m_HeartBeats = new FastCollection<string, HeartBeat>();
        readonly FastCollection<string, CountBeat> m_CountBeats = new FastCollection<string, CountBeat>();
        readonly List<string> m_DeleteHeartBeatNextFrame = new List<string>();
        readonly List<string> m_DeleteCountBeatNextFrame = new List<string>();
        readonly ObjectPool<HeartBeat> m_HeartBeatPool = new ObjectPool<HeartBeat>();
        readonly ObjectPool<CountBeat> m_CountBeatPool = new ObjectPool<CountBeat>();

        /// <summary>
        /// 注册全局心跳函数
        /// </summary>
        /// <param name="callbackName"></param>
        /// <param name="function"></param>
        /// <param name="count">心跳次数</param>
        /// <param name="intervalSeconds">间隔时间(秒)</param>
        public void RegisterCountBeat(
            string callbackName,
            GlobalCountBeatFunction function,
            int count,
            float intervalSeconds)
        {
            if (string.IsNullOrEmpty(callbackName) || function == null)
            {
                Log.InternalError("callback name is null or empty, try to apply an invalid name");
                return;
            }

            CountBeat countBeat = m_CountBeatPool.Get();
            countBeat.Name = callbackName;
            countBeat.Function = function;
            countBeat.Interval = intervalSeconds;
            countBeat.BeatCount = count;
            countBeat.State = EHearBeatState.Beating;
            m_CountBeats.Add(callbackName, countBeat);
        }

        /// <summary>
        /// 注册全局心跳函数
        /// </summary>
        /// <param name="callbackName"></param>
        /// <param name="function"></param>
        /// <param name="interval">间隔时间(秒)</param>
        /// <param name="durationSecond">总时长(秒)</param>
        public void RegisterHeartBeat(
            string callbackName,
            GlobalHeartBeatFunction function,
            float interval,
            float durationSecond)
        {
            if (string.IsNullOrEmpty(callbackName) || function == null)
            {
                Log.InternalError("callback name is null or empty, try to apply an invalid name");
                return;
            }

            HeartBeat heartBeat = m_HeartBeatPool.Get();
            heartBeat.Name = callbackName;
            heartBeat.Function = function;
            heartBeat.Interval = interval;
            heartBeat.Duration = durationSecond;
            heartBeat.State = EHearBeatState.Beating;
            m_HeartBeats.Add(callbackName, heartBeat);
        }

        /// <summary>
        /// 查找心跳函数
        /// </summary>
        /// <param name="callbackName"></param>
        /// <returns></returns>
        public HeartBeat FindHeartBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                return Empty<HeartBeat>.Value;
            }

            if (m_HeartBeats.TryGet(callbackName, out HeartBeat heartBeat))
            {
                return heartBeat;
            }

            Log.InternalError($"Heart beat name {callbackName} is not Registered");
            return Empty<HeartBeat>.Value;
        }

        /// <summary>
        /// 查找心跳函数
        /// </summary>
        /// <param name="callbackName"></param>
        /// <returns></returns>
        public CountBeat FindCountBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                return Empty<CountBeat>.Value;
            }

            if (m_CountBeats.TryGet(callbackName, out CountBeat countBeat))
            {
                return countBeat;
            }

            Log.InternalError($"Count beat name {callbackName} is not Registered");
            return Empty<CountBeat>.Value;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="callbackName"></param>
        public void PauseHeartBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError($"Heart beat name {callbackName} is not Registered");
                return;
            }

            if (!m_HeartBeats.TryGet(callbackName, out HeartBeat heartBeat))
            {
                return;
            }

            heartBeat.State = EHearBeatState.Idle;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="callbackName"></param>
        public void PauseCountBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError($"Count beat name {callbackName} is not Registered");
                return;
            }

            if (m_CountBeats.TryGet(callbackName, out CountBeat countBeat))
            {
                return;
            }

            countBeat.State = EHearBeatState.Idle;
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="callbackName"></param>
        public void RemoveHeartBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError($"Heart beat name {callbackName} is not Registered");
                return;
            }

            if (!m_HeartBeats.TryGet(callbackName, out HeartBeat heartBeat))
            {
                return;
            }

            m_DeleteHeartBeatNextFrame.Add(heartBeat.Name);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="callbackName"></param>
        public void RemoveCountBeat(string callbackName)
        {
            if (string.IsNullOrEmpty(callbackName))
            {
                Log.InternalError($"Count beat name {callbackName} is not Registered");
                return;
            }

            if (m_CountBeats.TryGet(callbackName, out CountBeat countBeat))
            {
                return;
            }

            m_DeleteCountBeatNextFrame.Add(countBeat.Name);
        }

        public override void Update(float deltaTime)
        {
            //检查删除
            if (m_DeleteHeartBeatNextFrame.Count > 0)
            {
                for (int i = 0; i < m_DeleteHeartBeatNextFrame.Count; i++)
                {
                    string callback = m_DeleteHeartBeatNextFrame[i];
                    if (m_HeartBeats.Contains(callback))
                    {
                        m_HeartBeats.Remove(callback);
                    }
                }

                m_DeleteHeartBeatNextFrame.Clear();
            }

            if (m_DeleteCountBeatNextFrame.Count > 0)
            {
                for (int i = 0; i < m_DeleteCountBeatNextFrame.Count; i++)
                {
                    string callback = m_DeleteCountBeatNextFrame[i];
                    if (m_CountBeats.Contains(callback))
                    {
                        m_CountBeats.Remove(callback);
                    }
                }

                m_DeleteCountBeatNextFrame.Clear();
            }

            UpdateHeartBeat(deltaTime);
            UpdateCountBeat(deltaTime);
        }

        void UpdateHeartBeat(float deltaTime)
        {
            foreach (HeartBeat heartBeat in m_HeartBeats.All)
            {
                //Idle不更新
                if (heartBeat.State == EHearBeatState.Idle)
                {
                    continue;
                }

                //记录开始时间
                if (heartBeat.LastTime < 0)
                {
                    heartBeat.LastTime = Time.time;
                    continue;
                }

                //计算时间差
                float diff = Time.time - heartBeat.LastTime;

                //超过时长删除
                if (heartBeat.Duration >= 0 && diff >= heartBeat.Duration)
                {
                    m_DeleteHeartBeatNextFrame.Add(heartBeat.Name);
                    continue;
                }

                //大于interval执行回调
                heartBeat.IntervalRunningTime += deltaTime;
                if (heartBeat.IntervalRunningTime >= heartBeat.Interval)
                {
                    heartBeat.IntervalRunningTime = 0;
                    heartBeat.Invoke();
                }
            }
        }

        void UpdateCountBeat(float deltaTime)
        {
            foreach (CountBeat heartBeat in m_CountBeats.All)
            {
                //Idle不更新
                if (heartBeat.State == EHearBeatState.Idle)
                {
                    continue;
                }

                //记录开始时间
                if (heartBeat.LastTime <= 0)
                {
                    heartBeat.LastTime = Time.time;
                    continue;
                }

                //超过计数删除
                if (heartBeat.BeatCount > 0 && heartBeat.CurCount > heartBeat.BeatCount)
                {
                    m_DeleteCountBeatNextFrame.Add(heartBeat.Name);
                    continue;
                }

                //大于interval执行回调
                heartBeat.IntervalRunningTime += deltaTime;
                if (heartBeat.IntervalRunningTime >= heartBeat.Interval)
                {
                    heartBeat.IntervalRunningTime = 0;
                    heartBeat.Invoke();
                    heartBeat.CurCount += 1;
                }
            }
        }
    }
}