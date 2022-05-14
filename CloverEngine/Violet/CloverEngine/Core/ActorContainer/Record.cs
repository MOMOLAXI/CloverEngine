﻿﻿﻿// ﻿﻿﻿using System.Collections.Generic;
//
// namespace Clover
// {
//     internal class Record : IRecord
//     {
//         // 属主
//         readonly Actor m_Owner;
//         // 表结构
//         readonly CRecData m_RecStructData;
//         // 数据
//         readonly List<Var[]> m_Datas = new List<Var[]>();
//         // 是否已经改变
//         bool m_HasChanged;
//
//         public string name => m_RecStructData.RecName;
//         public int colCount => m_RecStructData.ColCount;
//         public VarType[] colTypes { get; private set; }
//         public int rowCount => m_Datas.Count;
//
//         public List<RecordChangeFunction> hookList { get; set; }
//         public List<RecordLateChangeFunction> lateHookList { get; set; }
//
//         public Record(Actor owner, CRecData recData)
//         {
//             m_Owner = owner;
//             m_RecStructData = recData;
//             colTypes = m_RecStructData.ColTypes;
//         }
//
//         public bool AddRow(IVarList varList)
//         {
//             return AddRow(m_Datas.Count, varList);
//         }
//
//         public bool AddRow(int row, IVarList varList)
//         {
//             // global::Log.Assert(varList != null, "varList != null");
//
//             if (row < 0 || row > m_Datas.Count)
//             {
//                 // global::Log.LogErrorF("[Record:AddRow]add row overflow, current Count {0} but add {1}", m_Datas.Count, row);
//                 return false;
//             }
//
//             Var[] rowData = new Var[colCount];
//             int index = 0;
//
//             for (int col = 0; col < colCount; ++col)
//             {
//                 Var var = varList.GetVar(index++);
//                 if (var.type != colTypes[col])
//                 {
//                     return false;
//                 }
//
//                 rowData[col] = var;
//             }
//
//             if (row < m_Datas.Count)
//             {
//                 m_Datas.Insert(row, rowData);
//             }
//             else
//             {
//                 m_Datas.Add(rowData);
//             }
//
//             ExecuteHooks(RecOPType.AddRow, row, 0);
//
//             return true;
//         }
//
//         public bool AddRow(int row, CLoadArchive ar, StringBuffer sc)
//         {
//             // global::Log.Assert(ar != null, "ar != null");
//
//             if (row < 0 || row > m_Datas.Count)
//             {
//                 // global::Log.LogErrorF("[Record:AddRow]add row overflow, current Count {0} but add {1}", m_Datas.Count, row);
//                 return false;
//             }
//
//             Var[] rowData = new Var[colCount];
//             Var var = Var.zero;
//
//             for (int col = 0; col < colCount; ++col)
//             {
//                 OuterVarType outerVarType = m_RecStructData.OuterColTypes[col];
//                 if (!ar.ReadVar(outerVarType, sc, ref var))
//                 {
//                     // global::Log.LogErrorF("[Record:AddRow]read data failed, current row {0} col {1}", row, col);
//                     return false;
//                 }
//
//                 rowData[col] = var;
//             }
//
//             if (row < m_Datas.Count)
//             {
//                 m_Datas.Insert(row, rowData);
//             }
//             else
//             {
//                 m_Datas.Add(rowData);
//             }
//
//             ExecuteHooks(RecOPType.AddRow, row, 0);
//
//             return true;
//         }
//
//         public bool RemoveRow(int row)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 // global::Log.LogErrorF("[Record:RemoveRow]remove row overflow, current Count {0} but remove {1}", m_Datas.Count, row);
//                 return false;
//             }
//
//             Var[] data = m_Datas[row];
//             m_Datas.RemoveAt(row);
//
//             VarList old = VarList.AllocAutoVarList();
//             for (int i = 0; i < data.Length; ++i)
//             {
//                 old.AddVar(data[i]);
//             }
//
//             ExecuteHooks(RecOPType.RemoveRow, row, 0, old);
//
//             return true;
//         }
//
//         public void Clear()
//         {
//             if (m_Datas.Count > 0)
//             {
//                 m_Datas.Clear();
//                 ExecuteHooks(RecOPType.Clear, 0, 0);
//             }
//         }
//
//         public bool SetVarFromArchive(CLoadArchive ar)
//         {
//             // global::Log.Assert(ar != null, "ar != null");
//
//             int row = 0;
//             int col = 0;
//             Var var = Var.zero;
//
//             if (!ar.ReadInt16(ref row))
//             {
//                 return false;
//             }
//
//             if (!ar.ReadInt8(ref col))
//             {
//                 return false;
//             }
//
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return false;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return false;
//             }
//
//             if (!ar.ReadVar(m_RecStructData.OuterColTypes[col], null, ref var))
//             {
//                 return false;
//             }
//
//             return SetVar(row, col, var);
//         }
//
//         public bool SetRowVars(int row, IVarList indexAndVars)
//         {
//             // global::Log.Assert(indexAndVars != null);
//
//             if (indexAndVars == null)
//             {
//                 return false;
//             }
//
//             // 记录修改前的数据
//             VarList oldArgs = VarList.AllocAutoVarList();
//             Var[] oldVars = GetRowVars(row);
//             if (oldVars != null)
//             {
//                 for (int i = 0; i < oldVars.Length; ++i)
//                 {
//                     oldArgs.AddVar(oldVars[i]);
//                 }
//             }
//
//             // 修改新值
//             int count = indexAndVars.Count / 2;
//             for (int i = 0; i < count; ++i)
//             {
//                 int col = indexAndVars.GetInt32(i * 2);
//                 Var var = indexAndVars.GetVar(i * 2 + 1);
//
//                 SetVar(row, col, var);
//             }
//
//             ExecuteHooks(RecOPType.RowValueChange, row, 0, oldArgs);
//
//             return true;
//         }
//
//         public bool SetVar(int row, int col, Var var)
//         {
//             switch (var.type)
//             {
//                 case VarType.Bool:
//                      return SetBool(row, col, var.boolValue);
//                 case VarType.Int32:
//                     return SetInt(row, col, var.intValue);
//                 case VarType.Int64:
//                     return SetInt64(row, col, var.longValue);
//                 case VarType.Float:
//                     return SetFloat(row, col, var.floatValue);
//                 case VarType.Double:
//                     return SetDouble(row, col, var.doubleValue);
//                 case VarType.String:
//                     return SetString(row, col, var.stringValue);
//                 case VarType.Object:
//                     return SetObject(row, col, var.actorValue);
//             }
//
//             return false;
//         }
//
//         public bool SetBool(int row, int col, bool value)
//         {
//             return SetInt(row, col, value ? 1 : 0);
//         }
//
//         public bool SetInt(int row, int col, int value)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return false;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return false;
//             }
//
//             if (colTypes[col] != VarType.Int32)
//             {
//                 return false;
//             }
//
//             Var[] rowData = m_Datas[row];
//             if (rowData[col].intValue != value)
//             {
//                 int oldValue = rowData[col].intValue;
//                 rowData[col] = new Var(value);
//                 ExecuteHooks(RecOPType.ValueChange, row, col, VarList.AllocAutoVarList() < oldValue);
//             }
//
//             return true;
//         }
//
//         public bool SetInt64(int row, int col, long value)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return false;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return false;
//             }
//
//             if (colTypes[col] != VarType.Int64)
//             {
//                 return false;
//             }
//
//             Var[] rowData = m_Datas[row];
//             if (rowData[col].longValue != value)
//             {
//                 long oldValue = rowData[col].longValue;
//                 rowData[col] = new Var(value);
//                 ExecuteHooks(RecOPType.ValueChange, row, col, VarList.AllocAutoVarList() < oldValue);
//             }
//
//             return true;
//         }
//
//         public bool SetFloat(int row, int col, float value)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return false;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return false;
//             }
//
//             if (colTypes[col] != VarType.Float)
//             {
//                 return false;
//             }
//
//             Var[] rowData = m_Datas[row];
//             if (rowData[col].floatValue != value)
//             {
//                 float oldValue = rowData[col].floatValue;
//                 rowData[col] = new Var(value);
//                 ExecuteHooks(RecOPType.ValueChange, row, col, VarList.AllocAutoVarList() < oldValue);
//             }
//
//             return true;
//         }
//
//         public bool SetDouble(int row, int col, double value)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return false;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return false;
//             }
//
//             if (colTypes[col] != VarType.Double)
//             {
//                 return false;
//             }
//
//             Var[] rowData = m_Datas[row];
//             if (rowData[col].doubleValue != value)
//             {
//                 double oldValue = rowData[col].doubleValue;
//                 rowData[col] = new Var(value);
//                 ExecuteHooks(RecOPType.ValueChange, row, col, VarList.AllocAutoVarList() < oldValue);
//             }
//
//             return true;
//         }
//
//         public bool SetString(int row, int col, string value)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return false;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return false;
//             }
//
//             if (colTypes[col] != VarType.String)
//             {
//                 return false;
//             }
//
//             Var[] rowData = m_Datas[row];
//             if (rowData[col].stringValue != value)
//             {
//                 string oldValue = rowData[col].stringValue;
//                 rowData[col] = new Var(value);
//                 ExecuteHooks(RecOPType.ValueChange, row, col, VarList.AllocAutoVarList() < oldValue);
//             }
//
//             return true;
//         }
//
//         public bool SetObject(int row, int col, ActorID value)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return false;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return false;
//             }
//
//             if (colTypes[col] != VarType.Object)
//             {
//                 return false;
//             }
//
//             Var[] rowData = m_Datas[row];
//             if (rowData[col].actorValue != value)
//             {
//                 ActorID oldValue = rowData[col].actorValue;
//                 rowData[col] = new Var(value);
//                 ExecuteHooks(RecOPType.ValueChange, row, col, VarList.AllocAutoVarList() < oldValue);
//             }
//
//             return true;
//         }
//
//         public Var GetVar(int row, int col)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return Var.zero;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return Var.zero;
//             }
//
//             return m_Datas[row][col];
//         }
//
//         public Var[] GetRowVars(int row)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return null;
//             }
//
//             return m_Datas[row];
//         }
//
//         public bool GetBool(int row, int col)
//         {
//             return GetInt(row, col) != 0;
//         }
//
//         public int GetInt(int row, int col)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return 0;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return 0;
//             }
//
//             return m_Datas[row][col].intValue;
//         }
//
//         public long GetInt64(int row, int col)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return 0;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return 0;
//             }
//
//             return m_Datas[row][col].longValue;
//         }
//
//         public float GetFloat(int row, int col)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return 0;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return 0;
//             }
//
//             return m_Datas[row][col].floatValue;
//         }
//
//         public double GetDouble(int row, int col)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return 0;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return 0;
//             }
//
//             return m_Datas[row][col].doubleValue;
//         }
//
//         public string GetString(int row, int col)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return string.Empty;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return string.Empty;
//             }
//
//             return m_Datas[row][col].stringValue;
//         }
//
//         public ActorID GetObject(int row, int col)
//         {
//             if (row < 0 || row >= m_Datas.Count)
//             {
//                 return ActorID.zero;
//             }
//
//             if (col < 0 || col >= colCount)
//             {
//                 return ActorID.zero;
//             }
//
//             return m_Datas[row][col].actorValue;
//         }
//
//         public int FindBool(int col, bool value, int startRow)
//         {
//             if (col < 0 || col >= colCount)
//             {
//                 return -1;
//             }
//
//             for (int i = 0; i < m_Datas.Count; ++i)
//             {
//                 if (m_Datas[i][col].boolValue == value)
//                 {
//                     return i;
//                 }
//             }
//
//             return -1;
//         }
//
//         public int FindInt(int col, int value, int startRow)
//         {
//             if (col < 0 || col >= colCount)
//             {
//                 return -1;
//             }
//
//             for (int i = 0; i < m_Datas.Count; ++i)
//             {
//                 if (m_Datas[i][col].intValue == value)
//                 {
//                     return i;
//                 }
//             }
//
//             return -1;
//         }
//
//         public int FindInt64(int col, long value, int startRow)
//         {
//             if (col < 0 || col >= colCount)
//             {
//                 return -1;
//             }
//
//             for (int i = 0; i < m_Datas.Count; ++i)
//             {
//                 if (m_Datas[i][col].longValue == value)
//                 {
//                     return i;
//                 }
//             }
//
//             return -1;
//         }
//
//         public int FindFloat(int col, float value, int startRow)
//         {
//             if (col < 0 || col >= colCount)
//             {
//                 return -1;
//             }
//
//             for (int i = 0; i < m_Datas.Count; ++i)
//             {
//                 if (m_Datas[i][col].floatValue == value)
//                 {
//                     return i;
//                 }
//             }
//
//             return -1;
//         }
//
//         public int FindDouble(int col, double value, int startRow)
//         {
//             if (col < 0 || col >= colCount)
//             {
//                 return -1;
//             }
//
//             for (int i = 0; i < m_Datas.Count; ++i)
//             {
//                 if (m_Datas[i][col].doubleValue == value)
//                 {
//                     return i;
//                 }
//             }
//
//             return -1;
//         }
//
//         public int FindString(int col, string value, int startRow)
//         {
//             if (col < 0 || col >= colCount)
//             {
//                 return -1;
//             }
//
//             for (int i = 0; i < m_Datas.Count; ++i)
//             {
//                 if (m_Datas[i][col].stringValue == value)
//                 {
//                     return i;
//                 }
//             }
//
//             return -1;
//         }
//
//         public int FindObject(int col, ActorID value, int startRow)
//         {
//             if (col < 0 || col >= colCount)
//             {
//                 return -1;
//             }
//
//             for (int i = 0; i < m_Datas.Count; ++i)
//             {
//                 if (m_Datas[i][col].actorValue == value)
//                 {
//                     return i;
//                 }
//             }
//
//             return -1;
//         }
//
//         public void ExecuteHooks(RecOPType opType, int row, int col, IVarList old = null)
//         {
//             SetDirty();
//
//             if (hookList == null)
//             {
//                 return;
//             }
//
//             if (old == null)
//             {
//                 old = VarList.AllocAutoVarList();
//             }
//
//             for (int i = 0; i < hookList.Count; ++i)
//             {
//                 RecordChangeFunction pcb = hookList[i];
//
//                 try
//                 {
//                     pcb(m_Owner.ID, name, opType, row, col, old);
//                 }
//                 catch (System.Exception ex)
//                 {
//                     Log.InternalException(ex);
//                 }
//             }
//         }
//
//         public void Update()
//         {
//             if (!m_HasChanged)
//             {
//                 return;
//             }
//
//             m_HasChanged = false;
//
//             ExecuteLateHooks();
//         }
//
//         void InitColTypes()
//         {
//             if (m_RecStructData.ColTypes != null)
//             {
//                 colTypes = m_RecStructData.ColTypes;
//             }
//             else if (m_RecStructData.OuterColTypes != null)
//             {
//                 for (int i = 0; i < m_RecStructData.OuterColTypes.Length; ++i)
//                 {
//                     switch (m_RecStructData.OuterColTypes[i])
//                     {
//                         case OuterVarType.Byte:
//                         case OuterVarType.Word:
//                         case OuterVarType.DWord:
//                             {
//                                 colTypes[i] = VarType.Int32;
//                             }
//                             break;
//                         case OuterVarType.QWard:
//                             {
//                                 colTypes[i] = VarType.Int64;
//                             }
//                             break;
//                         case OuterVarType.Float:
//                             {
//                                 colTypes[i] = VarType.Float;
//                             }
//                             break;
//                         case OuterVarType.Double:
//                             {
//                                 colTypes[i] = VarType.Double;
//                             }
//                             break;
//                         case OuterVarType.String:
//                         case OuterVarType.WideStr:
//                             {
//                                 colTypes[i] = VarType.String;
//                             }
//                             break;
//                         case OuterVarType.Object:
//                             {
//                                 colTypes[i] = VarType.Object;
//                             }
//                             break;
//                     }
//                 }
//             }
//         }
//
//         void ExecuteLateHooks()
//         {
//             if (lateHookList == null)
//             {
//                 return;
//             }
//
//             for (int i = 0; i < lateHookList.Count; ++i)
//             {
//                 RecordLateChangeFunction pcb = lateHookList[i];
//
//                 try
//                 {
//                     pcb(m_Owner.ID, name);
//                 }
//                 catch (System.Exception ex)
//                 {
//                     Log.InternalException(ex);
//                 }
//             }
//         }
//
//         void SetDirty()
//         {
//             if (lateHookList != null)
//             {
//                 m_HasChanged = true;
//                 m_Owner.SetRecordDirty();
//             }
//         }
//     }
// }
