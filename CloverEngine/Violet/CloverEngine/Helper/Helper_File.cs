﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Clover
{
    public static partial class Helper
    {
        public const string EXTENSION_JSON = ".json";
        public const string EXTENSION_XML = ".xml";
        
        static readonly Dictionary<Type, XmlSerializer> s_XmlSerializers = new Dictionary<Type, XmlSerializer>();

        public static void CreateDirectory(params string[] directories)
        {
            if (directories == null)
            {
                return;
            }

            for (int i = 0; i < directories.Length; i++)
            {
                string directory = directories[i];
                if (Directory.Exists(directory))
                {
                    continue;
                }

                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception e)
                {
                    Log.InternalException(e);
                }
            }
        }

        /// <summary>
        /// 获取paths列表对应路径
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string ToPath(params string[] paths)
        {
            if (paths == null)
            {
                return string.Empty;
            }

            string ret = string.Empty;
            for (int i = 0, next = 1; i < paths.Length && next < paths.Length; i++, next++)
            {
                string path = paths[i];
                if (next < paths.Length)
                {
                    ret = Path.Combine(path, paths[next]);
                }
            }

            return ret;
        }

        /// <summary>
        /// 文件夹路径是否相同
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool IsSameDirectory(string d1, string d2)
        {
            string dir1 = Path.GetDirectoryName(d1);
            string dir2 = Path.GetDirectoryName(d2);
            if (string.IsNullOrEmpty(dir1) || string.IsNullOrEmpty(dir2))
            {
                return false;
            }

            return dir1 == dir2;
        }

        /// <summary>
        /// 上n级目录
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string GetParentDir(string dir, int n = 1)
        {
            string subDir = dir;

            for (int i = 0; i < n; ++i)
            {
                int last = subDir.LastIndexOf('/');
                subDir = subDir.Substring(0, last);
            }

            return subDir;
        }

        /// <summary>
        /// 保存到xml
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void SerializeXml(this object data, string path)
        {
            if (string.IsNullOrEmpty(path) || data == null)
            {
                return;
            }

            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            using (StreamWriter stream = new StreamWriter(path))
            {
                XmlSerializer serializer = GetXmlSerializer(data.GetType());
                serializer.Serialize(stream, data);
            }
        }

        /// <summary>
        /// 写入XML
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        public static void SerializeXml<T>(T data, string path) where T : class
        {
            if (string.IsNullOrEmpty(path) || data == null)
            {
                return;
            }

            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            using (StreamWriter stream = new StreamWriter(path))
            {
                XmlSerializer serializer = GetXmlSerializer<T>();
                serializer.Serialize(stream, data);
            }
        }

        /// <summary>
        /// 从Xml加载对象，没有则会创建xml
        /// </summary>
        /// <param name="absPath">绝对路径</param>
        /// <param name="autoCreate">是否自动创建</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeserializeXml<T>(string absPath, bool autoCreate = true) where T : class, new()
        {
            if (string.IsNullOrEmpty(absPath))
            {
                return Empty<T>.Value;
            }

            XmlSerializer serializer = GetXmlSerializer<T>();
            if (autoCreate)
            {
                string directory = Path.GetDirectoryName(absPath);
                if (directory != null)
                {
                    //检查文件夹
                    if (!File.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    //检查文件
                    if (!File.Exists(absPath))
                    {
                        using (StreamWriter stream = new StreamWriter(absPath))
                        {
                            serializer.Serialize(stream, new T());
                        }
                    }
                }
            }

            T ret = null;
            using (StreamReader stream = new StreamReader(absPath))
            {
                ret = serializer.Deserialize(stream) as T;
            }

            return ret;
        }

        public static XmlSerializer GetXmlSerializer(Type type)
        {
            if (type == null)
            {
                return default;
            }

            XmlSerializer serializer;
            if (s_XmlSerializers.ContainsKey(type))
            {
                serializer = s_XmlSerializers[type];
            }
            else
            {
                serializer = new XmlSerializer(type);
                s_XmlSerializers[type] = serializer;
            }

            return serializer;
        }

        /// <summary>
        /// 获取对象Xml序列化器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static XmlSerializer GetXmlSerializer<T>()
        {
            return GetXmlSerializer(typeof(T));
        }

        /// <summary>
        /// 是否是目录文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectory(string path)
        {
            return !string.IsNullOrEmpty(path) && Directory.Exists(path);
        }

        public static string XmlFile(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "Empty" + EXTENSION_XML;
            }

            return name + EXTENSION_XML;
        }

        /// <summary>
        /// 是否是非目录文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFile(string path)
        {
            return !string.IsNullOrEmpty(path) && !Directory.Exists(path) && File.Exists(path);
        }

        /// <summary>
        /// 是否是Json文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsJsonFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            return Path.GetExtension(path) == EXTENSION_JSON;
        }

        /// <summary>
        /// 是否是Xml文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsXmlFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            return Path.GetExtension(path) == EXTENSION_XML;
        }
    }
}