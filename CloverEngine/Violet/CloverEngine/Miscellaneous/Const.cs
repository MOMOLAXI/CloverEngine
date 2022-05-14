﻿namespace Clover
{
    public static class Const
    {
        public const int ACTOR_INITIAL_COUNT = 2000;

        public const int EXPAND_RATIO = 2;

        public const int DELAY_CREATE_COUNT_PRE_FRAME = 5;

        public const string ACTOR_NAME_FORMAT = "[{0}]{1}";

        public const string STREAMING_SCHEME = "file:///";

        //传输消息分割符
        public const byte MESSAGE_END_SUFFIX = 0xEE;

        //传输消息0xEE转义符
        public const byte MESSAGE_MID_SUFFIX = 0x00;

        //socket每次读取的消息长度
        public const int MAX_MESSAGE_LENGTH = 1024;

        //默认buffer缓冲区大小
        public const int DEFAULT_SOCKET_BUFFER_SIZE = 1024;

        //一次性验证串长度
        public const int VALIDATE_LEN_LENGTH = 1024;

        // 最大名字长度
        public const int OUTER_OBJNAME_LENGTH = 32;

        // 角色名最大长度
        public const int ROLENAME_MAX_LENGTH = 32;

        // 角度转弧度
        public const float DEG_TO_RAD = 0.0174533f;

        // 弧度转角度
        public const float RAD_TO_DEG = 57.2958f;
    }
}