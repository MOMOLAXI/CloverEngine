using UnityEngine;

namespace Clover
{
    public static partial class Helper
    {
        static readonly ParamReader s_ParamReader = new ParamReader();

        /// <summary>
        /// 通过字符串获取颜色 (r,g,b,a)
        /// </summary>
        /// <param name="colorStr"></param>
        /// <returns></returns>
        public static Color GetColorFromCommaSplit(string colorStr)
        {
            if (string.IsNullOrEmpty(colorStr))
            {
                return Color.clear;
            }

            s_ParamReader.SetStr(colorStr);
            float r = s_ParamReader.ReadFloat() / 255.0f;
            float g = s_ParamReader.ReadFloat() / 255.0f;
            float b = s_ParamReader.ReadFloat() / 255.0f;
            float a = s_ParamReader.ReadFloat() / 255.0f;
            return new Color(r, g, b, a);
        }
        
        
    }
}