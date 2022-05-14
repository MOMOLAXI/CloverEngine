﻿namespace Clover
{
    public enum EResType
    {
        None = -1,

        /// <summary>
        /// Clover引擎
        /// </summary>
        CloverEngine = 0,
        
        /// <summary>
        /// 引擎对象
        /// </summary>
        Actor,

        /// <summary>
        /// 角色
        /// </summary>
        Character,

        /// <summary>
        /// 特效
        /// </summary>
        Particle,

        /// <summary>
        /// 动画
        /// </summary>
        Animations,

        /// <summary>
        /// 场景
        /// </summary>
        Scene,

        /// <summary>
        /// 场景物件
        /// </summary>
        SceneAssist,

        /// <summary>
        /// 贴图
        /// </summary>
        Texture,

        /// <summary>
        /// AI
        /// </summary>
        AI,

        /// <summary>
        /// UI
        /// </summary>
        UI,

        /// <summary>
        /// 图集
        /// </summary>
        Atlas,

        /// <summary>
        /// Audio
        /// </summary>
        Audio,

        /// <summary>
        /// 字体
        /// </summary>
        Font,

        /// <summary>
        /// 材质
        /// </summary>
        Material,

        /// <summary>
        /// 剧情
        /// </summary>
        Story,

        /// <summary>
        /// 视频
        /// </summary>
        Video,

        /// <summary>
        /// 数据表 
        /// </summary>
        DataTable,
        
        /// <summary>
        /// Shader
        /// </summary>
        Shader,
        
        Max,
    }
}