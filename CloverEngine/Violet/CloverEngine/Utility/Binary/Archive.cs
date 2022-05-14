﻿﻿﻿// ﻿using System;
// using System.Text;
// using System.Runtime.InteropServices;
//
// namespace Clover
// {
//     // 读取字节数据流
//     internal class CLoadArchive
//     {
//         byte[] m_Data;
//         int m_Length;
//         int m_CurPosi;
//
//         readonly char[] m_TempData = new char[4096];
//
//         static readonly CLoadArchive s_AR = new CLoadArchive();
//
//         public CLoadArchive(byte[] pdata, int start, int len)
//         {
//             m_Data = new byte[len];
//             m_Length = len;
//             m_CurPosi = 0;
//
//             Array.Copy(pdata, start, m_Data, 0, len);
//         }
//
//         public CLoadArchive(byte[] pdata, int len)
//         {
//             m_Data = pdata;
//             m_Length = len;
//             m_CurPosi = 0;
//         }
//
//         public CLoadArchive(IntPtr pdata, int len)
//         {
//             m_Data = new byte[len];
//             m_Length = len;
//             m_CurPosi = 0;
//
//             Marshal.Copy(pdata, m_Data, 0, len);
//         }
//
//         protected CLoadArchive() { }
//
//         public static CLoadArchive Load(byte[] pdata, int len)
//         {
//             s_AR.m_Data = pdata;
//             s_AR.m_Length = len;
//             s_AR.m_CurPosi = 0;
//
//             return s_AR;
//         }
//
//         public static CLoadArchive Load(byte[] pdata, int offset, int len)
//         {
//             s_AR.m_Data = pdata;
//             s_AR.m_Length = len;
//             s_AR.m_CurPosi = offset;
//
//             return s_AR;
//         }
//
//         public CLoadArchive Clone()
//         {
//             byte[] data = new byte[m_Length];
//             Array.Copy(m_Data, 0, data, 0, m_Length);
//
//             CLoadArchive ar = new CLoadArchive(data, m_Length) {m_CurPosi = m_CurPosi};
//
//             return ar;
//         }
//
//         // 返回字节数组
//         public byte[] GetData()
//         {
//             return m_Data;
//         }
//
//         // 返回当前位置
//         public int GetOffset()
//         {
//             return m_CurPosi;
//         }
//
//         // 总长度
//         public int GetLength()
//         {
//             return m_Length;
//         }
//
//         // 移动到指定位置
//         public bool Seek(int posi)
//         {
//             if (posi < 0 || posi > m_Length)
//             {
//                 return false;
//             }
//
//             m_CurPosi = posi;
//
//             return true;
//         }
//
//         // 增加当前读取位置
//         public void IncPosi(int length)
//         {
//             m_CurPosi += length;
//
//             if (m_CurPosi > m_Length)
//             {
//                 throw new Exception();
//             }
//         }
//
//         // 读取数据
//         public bool ReadUInt8(ref uint val)
//         {
//             if (m_CurPosi + 1 > m_Length)
//             {
//                 return false;
//             }
//
//             val = m_Data[m_CurPosi];
//
//             IncPosi(1);
//
//             return true;
//         }
//
//         public bool ReadInt8(ref int val)
//         {
//             if (m_CurPosi + 1 > m_Length)
//             {
//                 return false;
//             }
//
//             val = m_Data[m_CurPosi];
//             if (val > 127)
//             {
//                 val -= 256;
//             }
//
//             IncPosi(1);
//
//             return true;
//         }
//
//         public bool ReadUInt16(ref uint val)
//         {
//             if (m_CurPosi + 2 > m_Length)
//             {
//                 return false;
//             }
//
//             val = BitConverter.ToUInt16(m_Data, m_CurPosi);
//
//             IncPosi(2);
//
//             return true;
//         }
//
//         public bool ReadInt16(ref int val)
//         {
//             if (m_CurPosi + 2 > m_Length)
//             {
//                 return false;
//             }
//
//             val = BitConverter.ToInt16(m_Data, m_CurPosi);
//
//             IncPosi(2);
//
//             return true;
//         }
//
//         public bool ReadUInt32(ref uint val)
//         {
//             if (m_CurPosi + 4 > m_Length)
//             {
//                 return false;
//             }
//
//             val = BitConverter.ToUInt32(m_Data, m_CurPosi);
//
//             IncPosi(4);
//
//             return true;
//         }
//
//         public bool ReadInt32(ref int val)
//         {
//             if (m_CurPosi + 4 > m_Length)
//             {
//                 return false;
//             }
//
//             val = BitConverter.ToInt32(m_Data, m_CurPosi);
//
//             IncPosi(4);
//
//             return true;
//         }
//
//         public bool ReadUInt64(ref ulong val)
//         {
//             if (m_CurPosi + 8 > m_Length)
//             {
//                 return false;
//             }
//
//             val = BitConverter.ToUInt64(m_Data, m_CurPosi);
//
//             IncPosi(8);
//
//             return true;
//         }
//
//         public bool ReadInt64(ref long val)
//         {
//             if (m_CurPosi + 8 > m_Length)
//             {
//                 return false;
//             }
//
//             val = BitConverter.ToInt64(m_Data, m_CurPosi);
//
//             IncPosi(8);
//
//             return true;
//         }
//
//         public bool ReadVarInt32(ref int val, int len)
//         {
//             if (m_CurPosi + len > m_Length)
//             {
//                 return false;
//             }
//
//             val = CompressInt.ReadInt32(m_Data, m_CurPosi, len);
//
//             IncPosi(len);
//
//             return true;
//         }
//
//         public bool ReadVarInt64(ref long val, int len)
//         {
//             if (m_CurPosi + len > m_Length)
//             {
//                 return false;
//             }
//
//             val = CompressInt.ReadInt64(m_Data, m_CurPosi, len);
//
//             IncPosi(len);
//
//             return true;
//         }
//
//         public bool ReadFloat(ref float val)
//         {
//             if (m_CurPosi + 4 > m_Length)
//             {
//                 return false;
//             }
//
//             val = BitConverter.ToSingle(m_Data, m_CurPosi);
//
//             IncPosi(4);
//
//             return true;
//         }
//
//         public bool ReadDouble(ref double val)
//         {
//             if (m_CurPosi + 8 > m_Length)
//             {
//                 return false;
//             }
//
//             val = BitConverter.ToDouble(m_Data, m_CurPosi);
//
//             IncPosi(8);
//
//             return true;
//         }
//
//         /// <summary>
//         /// 读取带前缀长度的UTF8字符串
//         /// </summary>
//         /// <param name="val"></param>
//         /// <returns></returns>
//         public bool ReadUTF8String(ref string val)
//         {
//             // 读取长度
//             uint len = 0;
//
//             if (!ReadUInt32(ref len))
//             {
//                 return false;
//             }
//
//             // 字符串
//             if (m_CurPosi + len > m_Length)
//             {
//                 return false;
//             }
//
//             if (len < 1)
//             {
//                 return false;
//             }
//
//             val = Encoding.UTF8.GetString(m_Data, m_CurPosi, (int) (len - 1));
//
//             if (val == null)
//             {
//                 val = string.Empty;
//                 return false;
//             }
//
//             IncPosi((int) len);
//
//             return true;
//         }
//
//         // 读取带前缀长度的字符串
//         public bool ReadString(ref string val)
//         {
//             // 读取长度
//             uint len = 0;
//
//             if (!ReadUInt32(ref len))
//             {
//                 return false;
//             }
//
//             // 字符串
//             if (m_CurPosi + len > m_Length)
//             {
//                 return false;
//             }
//
//             if (len < 1)
//             {
//                 return false;
//             }
//
//             val = Encoding.UTF8.GetString(m_Data, m_CurPosi, (int) (len - 1));
//
//             if (val == null)
//             {
//                 val = string.Empty;
//                 return false;
//             }
//
//             IncPosi((int) len);
//
//             return true;
//         }
//
//         public bool ReadString(StringBuffer sc, ref int id, ref string val)
//         {
//             // 读取长度
//             uint len = 0;
//
//             if (!ReadUInt32(ref len))
//             {
//                 return false;
//             }
//
//             // 字符串
//             if (m_CurPosi + len > m_Length)
//             {
//                 return false;
//             }
//
//             if (len < 1)
//             {
//                 return false;
//             }
//
//             if (sc != null)
//             {
//                 id = sc.Create(m_Data, m_CurPosi, (int) (len - 1));
//                 if (id != StringBuffer.INVALID_ID)
//                 {
//                     IncPosi((int) len);
//                     return true;
//                 }
//             }
//
//             val = Encoding.UTF8.GetString(m_Data, m_CurPosi, (int) (len - 1));
//
//             if (val == null)
//             {
//                 val = string.Empty;
//                 return false;
//             }
//
//             IncPosi((int) len);
//
//             return true;
//         }
//
//         // 读取不带前缀长度的UTF8字符串
//         public bool ReadUTF8StringNoLen(ref string val)
//         {
//             // 读取长度
//             uint len = 0;
//
//             for (int i = m_CurPosi; i < m_Length; i++)
//             {
//                 if (m_Data[i] == '\0')
//                 {
//                     len = (uint) (i - m_CurPosi + 1);
//                     break;
//                 }
//             }
//
//             // 字符串
//             if (m_CurPosi + len > m_Length)
//             {
//                 return false;
//             }
//
//             if (len < 1)
//             {
//                 return false;
//             }
//
//             val = Encoding.UTF8.GetString(m_Data, m_CurPosi, (int) (len - 1));
//
//             if (val == null)
//             {
//                 val = string.Empty;
//                 return false;
//             }
//
//             IncPosi((int) len);
//
//             return true;
//         }
//
//         // 读取不带前缀长度的字符串
//         public bool ReadStringNoLen(ref string val)
//         {
//             // 读取长度
//             uint len = 0;
//
//             for (int i = m_CurPosi; i < m_Length; i++)
//             {
//                 if (m_Data[i] == '\0')
//                 {
//                     len = (uint) (i - m_CurPosi + 1);
//                     break;
//                 }
//             }
//
//             // 字符串
//             if (m_CurPosi + len > m_Length)
//             {
//                 return false;
//             }
//
//             if (len < 1)
//             {
//                 return false;
//             }
//
//             val = Encoding.UTF8.GetString(m_Data, m_CurPosi, (int) (len - 1));
//
//             if (val == null)
//             {
//                 val = string.Empty;
//                 return false;
//             }
//
//             IncPosi((int) len);
//
//             return true;
//         }
//
//         // 读取带前缀长度的宽字符串
//         public bool ReadWideStr(ref string val)
//         {
//             uint len = 0;
//
//             if (!ReadUInt32(ref len))
//             {
//                 return false;
//             }
//
//             if (len < 2)
//             {
//                 return false;
//             }
//
//             return ReadUnicodeLen(ref val, (int) len);
//         }
//
//         // 读取不带前缀长度的宽字符串
//         public bool ReadWideStrNoLen(ref string val)
//         {
//             if (m_CurPosi > m_Length)
//             {
//                 return false;
//             }
//
//             int count = 0;
//             bool bHasEndSuffix = false;
//
//             for (int c = 0; c < m_Length; c = c + 2)
//             {
//                 char v = BitConverter.ToChar(m_Data, m_CurPosi + c);
//
//                 if (v == '\0')
//                 {
//                     bHasEndSuffix = true;
//                     count++;
//                     break;
//                 }
//
//                 count++;
//             }
//
//             if (!bHasEndSuffix)
//             {
//                 return false;
//             }
//
//             if (count < 1)
//             {
//                 return false;
//             }
//
//             char[] data = new char[count];
//             m_TempData[count] = '\0'; // 保护不会溢出
//             for (int c = 0; c < count; c++)
//             {
//                 char v = BitConverter.ToChar(m_Data, m_CurPosi + c * 2);
//
//                 if (v == '\0')
//                 {
//                     break;
//                 }
//
//                 data[c] = v;
//             }
//
//             val = new string(data, 0, count);
//
//             IncPosi(count * 2);
//
//             return true;
//         }
//
//         // 从指定长度中读取宽字符串
//         public bool ReadUnicodeLen(ref string val, int len)
//         {
//             if (m_CurPosi + len > m_Length)
//             {
//                 return false;
//             }
//
//             int size = len / 2;
//             char[] data;
//
//             if (size >= m_TempData.Length)
//             {
//                 data = new char[size];
//             }
//             else
//             {
//                 Array.Clear(m_TempData, 0, m_TempData.Length);
//                 data = m_TempData;
//             }
//
//             int count = 0;
//
//             for (int c = 0; c < size; c++)
//             {
//                 char v = BitConverter.ToChar(m_Data, m_CurPosi + c * 2);
//
//                 if (v == '\0')
//                 {
//                     break;
//                 }
//
//                 data[count++] = v;
//             }
//
//             val = new string(data, 0, count);
//
//             IncPosi(len);
//
//             return true;
//         }
//
//         // 对象ID
//         public bool ReadObject(ref ActorID val)
//         {
//             uint ident = 0;
//
//             if (!ReadUInt32(ref ident))
//             {
//                 return false;
//             }
//
//             uint serial = 0;
//
//             if (!ReadUInt32(ref serial))
//             {
//                 return false;
//             }
//
//             val = new ActorID(ident, serial);
//
//             return true;
//         }
//
//         // 二进制数据
//         public bool ReadUserData(ref byte[] val)
//         {
//             uint size = 0;
//
//             if (!ReadUInt32(ref size))
//             {
//                 return false;
//             }
//
//             if (m_CurPosi + size > m_Length)
//             {
//                 return false;
//             }
//
//             val = new byte[size];
//
//             Array.Copy(m_Data, m_CurPosi, val, 0, size);
//
//             IncPosi((int) size);
//
//             return true;
//         }
//
//         // 二进制数据 不带长度
//         public bool ReadUserDataNoLen(ref byte[] val, int len)
//         {
//             uint size = (uint) len;
//
//             if (m_CurPosi + size > m_Length)
//             {
//                 return false;
//             }
//
//             val = new byte[size];
//
//             Array.Copy(m_Data, m_CurPosi, val, 0, size);
//
//             IncPosi((int) size);
//
//             return true;
//         }
//
//         public bool ReadVarList(ref VarList varList)
//         {
//             uint count = 0;
//             if (!ReadUInt16(ref count))
//             {
//                 return false;
//             }
//
//             for (int i = 0; i < count; ++i)
//             {
//                 int type = 0;
//                 if (!ReadInt8(ref type))
//                 {
//                     return false;
//                 }
//
//                 switch ((VarType) type)
//                 {
//                     case VarType.Bool:
//                     {
//                         int value = 0;
//                         if (!ReadInt32(ref value))
//                         {
//                             return false;
//                         }
//
//                         varList.AddBool(value > 0);
//                     }
//                         break;
//                     case VarType.Int32:
//                     {
//                         int value = 0;
//                         if (!ReadInt32(ref value))
//                         {
//                             return false;
//                         }
//
//                         varList.AddInt(value);
//                     }
//                         break;
//                     case VarType.Int64:
//                     {
//                         long value = 0;
//                         if (!ReadInt64(ref value))
//                         {
//                             return false;
//                         }
//
//                         varList.AddInt64(value);
//                     }
//                         break;
//                     case VarType.Float:
//                     {
//                         float value = 0f;
//                         if (!ReadFloat(ref value))
//                         {
//                             return false;
//                         }
//
//                         varList.AddFloat(value);
//                     }
//                         break;
//                     case VarType.Double:
//                     {
//                         double value = 0;
//                         if (!ReadDouble(ref value))
//                         {
//                             return false;
//                         }
//
//                         varList.AddDouble(value);
//                     }
//                         break;
//                     case VarType.String:
//                     {
//                         string value = string.Empty;
//                         if (!ReadUTF8String(ref value))
//                         {
//                             return false;
//                         }
//
//                         varList.AddString(value);
//                     }
//                         break;
//                     case VarType.WideStr:
//                     {
//                         string value = string.Empty;
//                         if (!ReadWideStr(ref value))
//                         {
//                             return false;
//                         }
//
//                         varList.AddString(value);
//                     }
//                         break;
//                     case VarType.Object:
//                     {
//                         ActorID value = ActorID.zero;
//                         if (!ReadObject(ref value))
//                         {
//                             return false;
//                         }
//
//                         varList.AddObject(value);
//                     }
//                         break;
//                     case VarType.Binary:
//                     {
//                         byte[] data = null;
//                         if (!ReadUserData(ref data))
//                         {
//                             return false;
//                         }
//
//                         varList.AddBinary(data);
//                     }
//                         break;
//                     default:
//                         return false;
//                 }
//             }
//
//             return true;
//         }
//
//         public bool ReadVar(OuterVarType outerVarType, StringBuffer sc, ref Var var,
//             NetworkDataType dataType = NetworkDataType.UnKnow)
//         {
//             switch (outerVarType)
//             {
//                 case OuterVarType.Byte:
//                 {
//                     int value = 0;
//                     if (dataType == NetworkDataType.Empty)
//                     {
//                         var = new Var(0);
//                         return true;
//                     }
//
//                     if (dataType == NetworkDataType.UnKnow || dataType == NetworkDataType.Valid)
//                     {
//                         if (ReadInt8(ref value))
//                         {
//                             var = new Var(value);
//                             return true;
//                         }
//                     }
//                     else
//                     {
//                         if (ReadVarInt32(ref value, (int) dataType))
//                         {
//                             var = new Var(value);
//                             return true;
//                         }
//                     }
//                 }
//                     break;
//                 case OuterVarType.Word:
//                 {
//                     int value = 0;
//                     if (dataType == NetworkDataType.Empty)
//                     {
//                         var = new Var(0);
//                         return true;
//                     }
//
//                     if (dataType == NetworkDataType.UnKnow || dataType == NetworkDataType.Valid)
//                     {
//                         if (ReadInt16(ref value))
//                         {
//                             var = new Var(value);
//                             return true;
//                         }
//                     }
//                     else
//                     {
//                         if (ReadVarInt32(ref value, (int) dataType))
//                         {
//                             var = new Var(value);
//                             return true;
//                         }
//                     }
//                 }
//                     break;
//                 case OuterVarType.DWord:
//                 {
//                     int value = 0;
//                     if (dataType == NetworkDataType.Empty)
//                     {
//                         var = new Var(value);
//                         return true;
//                     }
//
//                     if (dataType == NetworkDataType.UnKnow || dataType == NetworkDataType.Valid)
//                     {
//                         if (ReadInt32(ref value))
//                         {
//                             var = new Var(value);
//                             return true;
//                         }
//                     }
//                     else
//                     {
//                         if (ReadVarInt32(ref value, (int) dataType))
//                         {
//                             var = new Var(value);
//                             return true;
//                         }
//                     }
//                 }
//                     break;
//                 case OuterVarType.QWard:
//                 {
//                     long value = 0;
//                     if (dataType == NetworkDataType.Empty)
//                     {
//                         var = new Var(value);
//                         return true;
//                     }
//
//                     if (dataType == NetworkDataType.UnKnow || dataType == NetworkDataType.Valid)
//                     {
//                         if (ReadInt64(ref value))
//                         {
//                             var = new Var(value);
//                             return true;
//                         }
//                     }
//                     else
//                     {
//                         if (ReadVarInt64(ref value, (int) dataType))
//                         {
//                             var = new Var(value);
//                             return true;
//                         }
//                     }
//                 }
//                     break;
//                 case OuterVarType.Float:
//                 {
//                     float value = 0;
//                     if (dataType == NetworkDataType.Empty)
//                     {
//                         var = new Var(value);
//                         return true;
//                     }
//
//                     if (ReadFloat(ref value))
//                     {
//                         var = new Var(value);
//                         return true;
//                     }
//                 }
//                     break;
//                 case OuterVarType.Double:
//                 {
//                     double value = 0;
//                     if (dataType == NetworkDataType.Empty)
//                     {
//                         var = new Var(value);
//                         return true;
//                     }
//
//                     if (ReadDouble(ref value))
//                     {
//                         var = new Var(value);
//                         return true;
//                     }
//                 }
//                     break;
//                 case OuterVarType.String:
//                 {
//                     int strID = StringBuffer.INVALID_ID;
//                     string value = string.Empty;
//
//                     if (dataType == NetworkDataType.Empty)
//                     {
//                         var = new Var(string.Empty);
//                         return true;
//                     }
//
//                     if (ReadString(sc, ref strID, ref value))
//                     {
//                         if (strID != StringBuffer.INVALID_ID)
//                         {
//                             var = new Var(sc, strID);
//                         }
//                         else
//                         {
//                             var = new Var(value);
//                         }
//
//                         return true;
//                     }
//                 }
//                     break;
//                 case OuterVarType.WideStr:
//                 {
//                     string value = string.Empty;
//
//                     if (dataType == NetworkDataType.Empty)
//                     {
//                         var = new Var(string.Empty);
//                         return true;
//                     }
//
//                     if (ReadWideStr(ref value))
//                     {
//                         var = new Var(value);
//                         return true;
//                     }
//                 }
//                     break;
//                 case OuterVarType.Object:
//                 {
//                     ActorID objID = ActorID.zero;
//
//                     if (dataType == NetworkDataType.Empty)
//                     {
//                         var = new Var(ActorID.zero);
//                         return true;
//                     }
//
//                     if (ReadObject(ref objID))
//                     {
//                         var = new Var(objID);
//                         return true;
//                     }
//                 }
//                     break;
//             }
//
//             return false;
//         }
//     }
//
//     // 写入字节数据流
//     public class CStoreArchive
//     {
//         byte[] m_Data;
//         int m_Length;
//         int m_CurPosi;
//
//         static readonly CStoreArchive s_AR = new CStoreArchive();
//
//         public CStoreArchive(int size)
//         {
//             m_Data = new byte[size];
//             m_Length = size;
//             m_CurPosi = 0;
//         }
//
//         public CStoreArchive(byte[] pdata, int size)
//         {
//             m_Data = pdata;
//             m_Length = size;
//             m_CurPosi = 0;
//
//             Array.Clear(m_Data, 0, size);
//         }
//
//         protected CStoreArchive() { }
//
//         public static CStoreArchive Load(byte[] pdata, int size)
//         {
//             s_AR.m_Data = pdata;
//             s_AR.m_Length = size;
//             s_AR.m_CurPosi = 0;
//
//             return s_AR;
//         }
//
//         public static CStoreArchive Load(byte[] pdata, int offset, int size)
//         {
//             s_AR.m_Data = pdata;
//             s_AR.m_Length = size;
//             s_AR.m_CurPosi = offset;
//
//             return s_AR;
//         }
//
//         // 返回字节数组
//         public byte[] GetData()
//         {
//             return m_Data;
//         }
//
//         // 返回长度
//         public int GetOffset()
//         {
//             return m_CurPosi;
//         }
//
//         // 返回容量
//         public int GetLength()
//         {
//             return m_Length;
//         }
//
//         // 移动到指定位置
//         public bool Seek(int posi)
//         {
//             if (posi < 0 || posi > m_Length)
//             {
//                 return false;
//             }
//
//             m_CurPosi = posi;
//
//             return true;
//         }
//
//         // 增加当前写入位置
//         public void IncPosi(int length)
//         {
//             m_CurPosi += length;
//
//             if (m_CurPosi > m_Length)
//             {
//                 throw new Exception();
//             }
//         }
//
//         // 申请必要的写入空间
//         private bool RequireLength(int length)
//         {
//             int need_len = m_CurPosi + length;
//
//             if (need_len <= m_Length)
//             {
//                 return true;
//             }
//
//             int new_len = m_Length * 2;
//
//             if (new_len < need_len)
//             {
//                 new_len = need_len;
//             }
//
//             byte[] new_buf = new byte[new_len];
//
//             Array.Copy(m_Data, new_buf, m_Length);
//
//             m_Data = new_buf;
//             m_Length = new_len;
//
//             return true;
//         }
//
//         // 写入数据
//         public bool WriteUInt8(uint val)
//         {
//             if (!RequireLength(1))
//             {
//                 return false;
//             }
//
//             m_Data[m_CurPosi] = (byte) (val & 0xFF);
//
//             IncPosi(1);
//
//             return true;
//         }
//
//         public bool WriteInt8(int val)
//         {
//             return WriteUInt8((uint) val);
//         }
//
//         public bool WriteUInt16(uint val)
//         {
//             if (!RequireLength(2))
//             {
//                 return false;
//             }
//
//             LRBitConverter.GetBytes((UInt16) val, m_Data, m_CurPosi);
//
//             IncPosi(2);
//
//             return true;
//         }
//
//         public bool WriteInt16(int val)
//         {
//             return WriteUInt16((uint) val);
//         }
//
//         public bool WriteUInt32(uint val)
//         {
//             if (!RequireLength(4))
//             {
//                 return false;
//             }
//
//             LRBitConverter.GetBytes(val, m_Data, m_CurPosi);
//
//             IncPosi(4);
//
//             return true;
//         }
//
//         public bool WriteInt32(int val)
//         {
//             return WriteUInt32((uint) val);
//         }
//
//         public bool WriteUInt64(ulong val)
//         {
//             if (!RequireLength(8))
//             {
//                 return false;
//             }
//
//             LRBitConverter.GetBytes(val, m_Data, m_CurPosi);
//
//             IncPosi(8);
//
//             return true;
//         }
//
//         public bool WriteInt64(long val)
//         {
//             return WriteUInt64((ulong) val);
//         }
//
//         public bool WriteFloat(float val)
//         {
//             if (!RequireLength(4))
//             {
//                 return false;
//             }
//
//             LRBitConverter.GetBytes(val, m_Data, m_CurPosi);
//
//             IncPosi(4);
//
//             return true;
//         }
//
//         public bool WriteDouble(double val)
//         {
//             if (!RequireLength(8))
//             {
//                 return false;
//             }
//
//             LRBitConverter.GetBytes(val, m_Data, m_CurPosi);
//
//             IncPosi(8);
//
//             return true;
//         }
//
//         // 写入带前缀长度的UTF8字符串
//         public bool WriteUTF8String(string val)
//         {
//             // 包含结束符
//             uint len = (uint) Encoding.UTF8.GetByteCount(val) + 1;
//
//             if (!RequireLength(4 + (int) len))
//             {
//                 return false;
//             }
//
//             m_Data[m_CurPosi] = (byte) (len & 0xFF);
//             m_Data[m_CurPosi + 1] = (byte) ((len >> 8) & 0xFF);
//             m_Data[m_CurPosi + 2] = (byte) ((len >> 16) & 0xFF);
//             m_Data[m_CurPosi + 3] = (byte) ((len >> 24) & 0xFF);
//
//             IncPosi(4);
//
//             try
//             {
//                 Encoding.UTF8.GetBytes(val).CopyTo(m_Data, m_CurPosi);
//             }
//             catch (Exception)
//             {
//                 return false;
//             }
//
//             // 结束符
//             m_Data[m_CurPosi + len - 1] = 0;
//
//             IncPosi((int) len);
//
//             return true;
//         }
//
//         // 写入UTF8字符串
//         public bool WriteUTF8StringNoLen(string val)
//         {
//             // 包含结束符
//             uint len = (uint) Encoding.UTF8.GetByteCount(val) + 1;
//
//             if (!RequireLength(4 + (int) len))
//             {
//                 return false;
//             }
//
//             try
//             {
//                 Encoding.UTF8.GetBytes(val).CopyTo(m_Data, m_CurPosi);
//             }
//             catch (Exception)
//             {
//                 return false;
//             }
//
//             // 结束符
//             m_Data[m_CurPosi + len - 1] = 0;
//
//             IncPosi((int) len);
//
//             return true;
//         }
//
//         // 写入带前缀长度的字符串
//         public bool WriteString(string val)
//         {
//             // 包含结束符
//             uint len = (uint) Encoding.UTF8.GetByteCount(val) + 1;
//
//             if (!RequireLength(4 + (int) len))
//             {
//                 return false;
//             }
//
//             m_Data[m_CurPosi] = (byte) (len & 0xFF);
//             m_Data[m_CurPosi + 1] = (byte) ((len >> 8) & 0xFF);
//             m_Data[m_CurPosi + 2] = (byte) ((len >> 16) & 0xFF);
//             m_Data[m_CurPosi + 3] = (byte) ((len >> 24) & 0xFF);
//
//             IncPosi(4);
//
//             try
//             {
//                 Encoding.UTF8.GetBytes(val).CopyTo(m_Data, m_CurPosi);
//             }
//             catch (Exception)
//             {
//                 return false;
//             }
//
//             // 结束符
//             m_Data[m_CurPosi + len - 1] = 0;
//
//             IncPosi((int) len);
//
//             return true;
//         }
//
//         // 写入不带前缀长度的字符串
//         public bool WriteStringNoLen(string val)
//         {
//             // 包含结束符
//             uint len = (uint) Encoding.UTF8.GetByteCount(val) + 1;
//
//             if (!RequireLength((int) len))
//             {
//                 return false;
//             }
//
//             try
//             {
//                 Encoding.UTF8.GetBytes(val).CopyTo(m_Data, m_CurPosi);
//             }
//             catch (Exception)
//             {
//                 return false;
//             }
//
//             // 结束符
//             m_Data[m_CurPosi + len - 1] = 0;
//
//             IncPosi((int) len);
//
//             return true;
//         }
//
//         // 写入带前缀长度的宽字符串
//         public bool WriteWideStr(string val)
//         {
//             // 包含结束符
//             int len = (val.Length + 1) * 2;
//
//             if (!RequireLength(4 + len))
//             {
//                 return false;
//             }
//
//             m_Data[m_CurPosi] = (byte) (len & 0xFF);
//             m_Data[m_CurPosi + 1] = (byte) ((len >> 8) & 0xFF);
//             m_Data[m_CurPosi + 2] = (byte) ((len >> 16) & 0xFF);
//             m_Data[m_CurPosi + 3] = (byte) ((len >> 24) & 0xFF);
//
//             IncPosi(4);
//
//             return WriteUnicodeLen(val, len);
//         }
//
//         // 写入带前缀长度的宽字符串
//         public bool WriteWideStrNoLen(string val)
//         {
//             // 包含结束符
//             int len = (val.Length + 1) * 2;
//
//             if (!RequireLength(len))
//             {
//                 return false;
//             }
//
//             return WriteUnicodeLen(val, len);
//         }
//
//         // 写入不超过指定长度的宽字符串
//         public bool WriteUnicodeLen(string val, int len)
//         {
//             if (!RequireLength(len))
//             {
//                 return false;
//             }
//
//             // 截断
//             int val_len = val.Length;
//
//             if (val_len >= len / 2)
//             {
//                 val_len = len / 2 - 1;
//             }
//
//             for (int c = 0; c < len; c++)
//             {
//                 m_Data[m_CurPosi + c] = 0;
//             }
//
//             for (int c = 0; c < val_len; c++)
//             {
//                 LRBitConverter.GetBytes(val[c], m_Data,
//                     m_CurPosi + c * 2);
//             }
//
//             IncPosi(len);
//
//             return true;
//         }
//
//         // 对象ID
//         public bool WriteObject(ActorID val)
//         {
//             if (!RequireLength(8))
//             {
//                 return false;
//             }
//
//             WriteUInt32(val.Ident);
//             WriteUInt32(val.Serial);
//
//             return true;
//         }
//
//         // 字符串类型，二进制数据
//         public bool WriteStringData(byte[] val)
//         {
//             uint len = (uint) val.Length + 1;
//
//             if (!RequireLength(4 + (int) len))
//             {
//                 return false;
//             }
//
//             WriteUInt32(len);
//
//             Array.Copy(val, 0, m_Data, m_CurPosi, val.Length);
//
//             // 结束符
//             m_Data[m_CurPosi + len - 1] = 0;
//
//             IncPosi((int) len);
//
//             return true;
//         }
//
//         // 二进制数据
//         public bool WriteUserData(byte[] val)
//         {
//             if (!RequireLength(val.Length + 4))
//             {
//                 return false;
//             }
//
//             WriteUInt32((uint) val.Length);
//
//             Array.Copy(val, 0, m_Data, m_CurPosi, val.Length);
//
//             IncPosi(val.Length);
//
//             return true;
//         }
//
//         // 二进制数据 不带长度
//         public bool WriteUserDataNoLen(byte[] val)
//         {
//             if (!RequireLength(val.Length))
//             {
//                 return false;
//             }
//
//             Array.Copy(val, 0, m_Data, m_CurPosi, val.Length);
//
//             IncPosi(val.Length);
//
//             return true;
//         }
//
//         //Write
//         public bool WriteVarList(IVarList val, bool bDiffStr = true)
//         {
//             if (!WriteInt16(val.Count))
//             {
//                 return false;
//             }
//
//             bool result = true;
//             int index = 0;
//
//             for (int i = 0; i < val.Count; ++i)
//             {
//                 Var var = val.GetVar(index++);
//
//                 // 字符串默认转成宽字符串
//                 VarType type = var.type;
//                 if (!bDiffStr)
//                 {
//                     type = var.type == VarType.String ? VarType.WideStr : var.type;
//                 }
//
//                 if (!WriteInt8((int) type))
//                 {
//                     return false;
//                 }
//
//                 switch (type)
//                 {
//                     case VarType.Bool:
//                         result = WriteInt32(var.boolValue ? 1 : 0);
//                         break;
//                     case VarType.Int32:
//                         result = WriteInt32(var.intValue);
//                         break;
//                     case VarType.Int64:
//                         result = WriteInt64(var.longValue);
//                         break;
//                     case VarType.Float:
//                         result = WriteFloat(var.floatValue);
//                         break;
//                     case VarType.Double:
//                         result = WriteDouble(var.doubleValue);
//                         break;
//                     case VarType.String:
//                         result = WriteUTF8String(var.stringValue);
//                         break;
//                     case VarType.WideStr:
//                         result = WriteWideStr(var.stringValue);
//                         break;
//                     case VarType.Object:
//                         result = WriteObject(var.actorValue);
//                         break;
//                 }
//
//                 if (!result)
//                 {
//                     return false;
//                 }
//             }
//
//             return true;
//         }
//     }
// }