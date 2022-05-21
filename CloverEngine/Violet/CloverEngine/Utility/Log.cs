using System;
using Debug = UnityEngine.Debug;

namespace Clover
{
    public enum LogLevel
    {
        None,
        Exception,
        Error,
        Info,
        Warning,
        Debug,

        Max,
    }

    public static class Log
    {
        public static LogLevel logLevel { set; get; } = LogLevel.Max;

        public static void LogDebug(object message)
        {
            if (logLevel < LogLevel.Debug)
            {
                return;
            }

            Debug.Log(message);
        }

        public static void Info(object message)
        {
            if (logLevel < LogLevel.Info)
            {
                return;
            }

            Debug.Log(message);
        }

        internal static void InternalInfo(object message)
        {
            if (logLevel < LogLevel.Error)
            {
                return;
            }

            Debug.Log($"{CloverEngine.ENGINE_LOG_TAG}{message}");
        }


        public static void Warning(object message)
        {
            if (logLevel < LogLevel.Warning)
            {
                return;
            }

            Debug.LogWarning(message);
        }

        public static void Error(object message)
        {
            if (logLevel < LogLevel.Error)
            {
                return;
            }

            Debug.LogError(message);
        }

        internal static void InternalError(object message)
        {
            if (logLevel < LogLevel.Error)
            {
                return;
            }

            Debug.LogError($"{CloverEngine.ENGINE_LOG_TAG}{message}");
        }

        internal static void InternalWarning(object message)
        {
            if (logLevel < LogLevel.Warning)
            {
                return;
            }

            Debug.LogWarning($"{CloverEngine.ENGINE_LOG_TAG}{message}");
        }

        public static void LogException(Exception ex)
        {
            if (logLevel < LogLevel.Exception)
            {
                return;
            }

            Debug.LogException(ex);
        }

        internal static void InternalException(Exception exception)
        {
            if (logLevel < LogLevel.Exception)
            {
                return;
            }

            Debug.LogError($"================={CloverEngine.ENGINE_LOG_TAG} Exception Start ===============");
            Debug.LogException(exception);
            Debug.LogError($"================={CloverEngine.ENGINE_LOG_TAG} Exception End ===============");
        }

        public static void UnexpectedError(string process)
        {
            InternalError($"Engine caught unexpected error while {process}");
        }
    }
}