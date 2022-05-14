﻿﻿﻿﻿using System;
using System.Collections;
using System.Reflection;

 namespace Clover
{
    public static partial class Helper
    {
        public static object DeepCloneObject(object objSource)
        {
            if (objSource == null)
            {
                return default;
            }

            object objTarget = Activator.CreateInstance(objSource.GetType());
            PropertyInfo[] propertyInfo = GetPropertyInfos(objSource.GetType(),
                BindingFlags.Public,
                BindingFlags.NonPublic,
                BindingFlags.Instance);

            //Assign all source property to target object's properties
            Foreach(propertyInfo, prop =>
            {
                //检查是否可写
                if (!prop.CanWrite)
                {
                    return;
                }

                //检查属性是否是值类型，枚举或者字符串
                Type propType = prop.PropertyType;
                if (propType.IsPrimitive || propType.IsEnum || propType == typeof(string))
                {
                    prop.SetValue(objTarget, prop.GetValue(objSource, null), null);
                    return;
                }

                //迭代器
                if (!typeof(IEnumerable).IsAssignableFrom(propType))
                {
                    return;
                }

                //Include List and Dictionary and......
                if (propType.IsGenericType)
                {
                    object enumerableObject = Activator.CreateInstance(propType);

                    MethodInfo addMethod = GetMethod(propType, "Add",
                        BindingFlags.Public,
                        BindingFlags.NonPublic,
                        BindingFlags.Instance);

                    if (!(prop.GetValue(objSource, null) is IEnumerable currentValues))
                    {
                        prop.SetValue(objTarget, null, null);
                    }
                    else
                    {
                        if (enumerableObject is IDictionary dictionary)
                        {
                            foreach (object currentValue in currentValues)
                            {
                                PropertyInfo propInfoKey = currentValue.GetType().GetProperty("Key");
                                PropertyInfo propInfoValue = currentValue.GetType().GetProperty("Value");
                                if (propInfoKey == null || propInfoValue == null)
                                {
                                    continue;
                                }

                                object keyValue = propInfoKey.GetValue(currentValue, null);
                                object valueValue = propInfoValue.GetValue(currentValue, null);
                                object finalKeyValue, finalValueValue;

                                //Get finalKeyValue
                                Type currentKeyType = keyValue.GetType();
                                if (currentKeyType.IsPrimitive || currentKeyType.IsEnum ||
                                    currentKeyType == typeof(string))
                                {
                                    finalKeyValue = keyValue;
                                }
                                else
                                {
                                    //Reference type
                                    object copyObj = DeepCloneObject(keyValue);
                                    finalKeyValue = copyObj;
                                }

                                //Get finalValueValue
                                Type currentValueType = valueValue.GetType();
                                if (currentValueType.IsPrimitive || currentValueType.IsEnum ||
                                    currentValueType == typeof(string))
                                {
                                    finalValueValue = valueValue;
                                }
                                else
                                {
                                    //Reference type
                                    object copyObj = DeepCloneObject(valueValue);
                                    finalValueValue = copyObj;
                                }

                                addMethod.Invoke(dictionary, new[] {finalKeyValue, finalValueValue});
                            }

                            prop.SetValue(objTarget, dictionary, null);
                        }
                        //Common IList type
                        else
                        {
                            foreach (object currentValue in currentValues)
                            {
                                Type currentType = currentValue.GetType();
                                if (currentType.IsPrimitive || currentType.IsEnum || currentType == typeof(string))
                                {
                                    addMethod.Invoke(enumerableObject, new[] {currentValue});
                                }
                                else
                                {
                                    //Reference type
                                    object copyObj = DeepCloneObject(currentValue);
                                    addMethod.Invoke(enumerableObject, new[] {copyObj});
                                }
                            }

                            prop.SetValue(objTarget, enumerableObject, null);
                        }
                    }
                }
                //Array type
                else
                {
                    if (!(prop.GetValue(objSource, null) is Array currentValues))
                    {
                        prop.SetValue(objTarget, null, null);
                    }
                    else
                    {
                        Array cloneObj = Activator.CreateInstance(prop.PropertyType, currentValues.Length) as Array;
                        ArrayList arrayList = new ArrayList();
                        for (int i = 0; i < currentValues.Length; i++)
                        {
                            Type currentType = currentValues.GetValue(i).GetType();
                            if (currentType.IsPrimitive || currentType.IsEnum || currentType == typeof(string))
                            {
                                arrayList.Add(currentValues.GetValue(i));
                            }
                            else
                            {
                                //Reference type
                                object copyObj = DeepCloneObject(currentValues.GetValue(i));
                                arrayList.Add(copyObj);
                            }
                        }

                        arrayList.CopyTo(cloneObj, 0);
                        prop.SetValue(objTarget, cloneObj, null);
                    }
                }
            });
            for (int index = 0; index < propertyInfo.Length; index++)
            {
                PropertyInfo property = propertyInfo[index];
                //Check whether property can be written to
                if (!property.CanWrite) continue;
                //check whether property type is value type, enum or string type
                if (property.PropertyType.IsPrimitive || property.PropertyType.IsEnum ||
                    property.PropertyType == typeof(string))
                {
                    property.SetValue(objTarget, property.GetValue(objSource, null), null);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    //Include List and Dictionary and......
                    if (property.PropertyType.IsGenericType)
                    {
                        object cloneObj = Activator.CreateInstance(property.PropertyType);

                        MethodInfo addMethod = property.PropertyType.GetMethod("Add",
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                        IEnumerable currentValues = property.GetValue(objSource, null) as IEnumerable;
                        if (currentValues == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            if (cloneObj is IDictionary)
                            {
                                cloneObj = cloneObj as IDictionary;
                                foreach (object currentValue in currentValues)
                                {
                                    PropertyInfo propInfoKey = currentValue.GetType().GetProperty("Key");
                                    PropertyInfo propInfoValue = currentValue.GetType().GetProperty("Value");
                                    if (propInfoKey == null || propInfoValue == null)
                                    {
                                        continue;
                                    }

                                    object keyValue = propInfoKey.GetValue(currentValue, null);
                                    object valueValue = propInfoValue.GetValue(currentValue, null);

                                    object finalKeyValue, finalValueValue;

                                    //Get finalKeyValue
                                    Type currentKeyType = keyValue.GetType();
                                    if (currentKeyType.IsPrimitive || currentKeyType.IsEnum ||
                                        currentKeyType == typeof(string))
                                    {
                                        finalKeyValue = keyValue;
                                    }
                                    else
                                    {
                                        //Reference type
                                        object copyObj = DeepCloneObject(keyValue);
                                        finalKeyValue = copyObj;
                                    }

                                    //Get finalValueValue
                                    Type currentValueType = valueValue.GetType();
                                    if (currentValueType.IsPrimitive || currentValueType.IsEnum ||
                                        currentValueType == typeof(string))
                                    {
                                        finalValueValue = valueValue;
                                    }
                                    else
                                    {
                                        //Reference type
                                        object copyObj = DeepCloneObject(valueValue);
                                        finalValueValue = copyObj;
                                    }

                                    addMethod.Invoke(cloneObj, new[] {finalKeyValue, finalValueValue});
                                }

                                property.SetValue(objTarget, cloneObj, null);
                            }
                            //Common IList type
                            else
                            {
                                foreach (object currentValue in currentValues)
                                {
                                    Type currentType = currentValue.GetType();
                                    if (currentType.IsPrimitive || currentType.IsEnum || currentType == typeof(string))
                                    {
                                        addMethod.Invoke(cloneObj, new[] {currentValue});
                                    }
                                    else
                                    {
                                        //Reference type
                                        object copyObj = DeepCloneObject(currentValue);
                                        addMethod.Invoke(cloneObj, new[] {copyObj});
                                    }
                                }

                                property.SetValue(objTarget, cloneObj, null);
                            }
                        }
                    }
                    //Array type
                    else
                    {
                        Array currentValues = property.GetValue(objSource, null) as Array;
                        if (null == currentValues)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            Array cloneObj =
                                Activator.CreateInstance(property.PropertyType, currentValues.Length) as Array;
                            ArrayList arrayList = new ArrayList();
                            for (int i = 0; i < currentValues.Length; i++)
                            {
                                Type currentType = currentValues.GetValue(i).GetType();
                                if (currentType.IsPrimitive || currentType.IsEnum || currentType == typeof(string))
                                {
                                    arrayList.Add(currentValues.GetValue(i));
                                }
                                else
                                {
                                    //Reference type
                                    object copyObj = DeepCloneObject(currentValues.GetValue(i));
                                    arrayList.Add(copyObj);
                                }
                            }

                            arrayList.CopyTo(cloneObj, 0);
                            property.SetValue(objTarget, cloneObj, null);
                        }
                    }
                }
                else
                {
                    object objPropertyValue = property.GetValue(objSource, null);
                    if (objPropertyValue == null)
                    {
                        property.SetValue(objTarget, null, null);
                    }
                    else if (objPropertyValue.GetType().IsPrimitive || objPropertyValue.GetType().IsEnum ||
                             objPropertyValue is string)
                    {
                        property.SetValue(objTarget, objPropertyValue, null);
                    }
                    else
                    {
                        property.SetValue(objTarget, DeepCloneObject(objPropertyValue), null);
                    }
                }
            }

            return objTarget;
        }


        public static PropertyInfo GetProperty(Type type, string name)
        {
            if (type == null || string.IsNullOrEmpty(name))
            {
                return null;
            }

            return type.GetProperty(name);
        }

        public static MethodInfo GetMethod(Type type, string name, params BindingFlags[] flags)
        {
            if (flags == null)
            {
                return type.GetMethod(name);
            }

            BindingFlags bindingFlags = BindingFlags.Default;
            Foreach(flags, flag => { bindingFlags |= flag; });
            return bindingFlags == BindingFlags.Default ? type.GetMethod(name) : type.GetMethod(name, bindingFlags);
        }

        public static PropertyInfo[] GetPropertyInfos<T>(params BindingFlags[] flags) where T : class
        {
            return GetPropertyInfos(typeof(T), flags);
        }

        public static PropertyInfo[] GetPropertyInfos(Type type, params BindingFlags[] flags)
        {
            if (type == null || flags == null)
            {
                return EmptyArray<PropertyInfo>.Value;
            }

            BindingFlags bindingFlags = BindingFlags.Default;
            Foreach(flags, flag => { bindingFlags |= flag; });
            return bindingFlags == BindingFlags.Default ? type.GetProperties() : type.GetProperties(bindingFlags);
        }
    }
}