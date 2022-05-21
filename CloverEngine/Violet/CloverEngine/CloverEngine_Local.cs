namespace Clover
{
    public static partial class CloverEngine
    {
        /// <summary>
        /// [Int] 获取local数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetInt(string key)
        {
            return CloverSetting.GetInt(key);
        }

        /// <summary>
        /// [float] 获取local数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static float GetFloat(string key)
        {
            return CloverSetting.GetFloat(key);
        }

        /// <summary>
        /// [string] 获取local数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            return CloverSetting.GetString(key);
        }

        /// <summary>
        /// 设置local数据 [int]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetLocalValue(string key, int value)
        {
            CloverSetting.Set(key, value);
        }

        /// <summary>
        /// 设置local数据 [float]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetLocalValue(string key, float value)
        {
            CloverSetting.Set(key, value);
        }

        /// <summary>
        /// 设置local数据 [string]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetLocalValue(string key, string value)
        {
            CloverSetting.Set(key, value);
        }
    }
}