﻿﻿﻿using System;
using System.Collections.Generic;
  using System.Xml.Serialization;
using UnityEngine;

namespace Clover
{
    [Serializable]
    public class CloverActorDefine : ScriptableObject
    {
        public const string Name = "ActorDefine.asset";
        
        public List<ActorDefine> defines = new List<ActorDefine>();

        public void RefreshDefines(List<ActorDefine> newDefines)
        {
            if (newDefines == null)
            {
                return;
            }

            defines.Clear();
            defines.AddRange(newDefines);
        }
    }

    /// <summary>
    /// 用一维加二维数据描述对象
    /// </summary>
    [XmlRoot("Actor"), Serializable]
    public class ActorDefine
    {
        [XmlAttribute("name")]
        public string name;

        [XmlAttribute("desc")]
        public string desc;

        [XmlElement("Properties")]
        public ActorPropertyCollection propertyCollection = new ActorPropertyCollection();

        [XmlElement("Tables")]
        public ActorTableCollection tableCollection = new ActorTableCollection();

        [XmlElement("Children")]
        public List<ActorDefine> children = new List<ActorDefine>();

        public void ForeachProperties(Run<ActorProperty> runner)
        {
            if (runner == null)
            {
                return;
            }

            for (int i = 0; i < propertyCollection.properties.Count; i++)
            {
                ActorProperty actorProperty = propertyCollection.properties[i];
                Function.Run(runner, actorProperty);
            }
        }

        public void ForeachTables(Run<ActorDataTable> runner)
        {
            if (runner == null)
            {
                return;
            }

            for (int i = 0; i < tableCollection.tables.Count; i++)
            {
                ActorDataTable dataTable = tableCollection.tables[i];
                Function.Run(runner, dataTable);
            }
        }

        public void AddProperty(ActorProperty property)
        {
            if (property == null)
            {
                return;
            }

            propertyCollection.properties.Add(property);
        }

        public void AddTable(ActorDataTable table)
        {
            if (table == null)
            {
                return;
            }

            tableCollection.tables.Add(table);
        }

        public void AddChild(ActorDefine define)
        {
            if (define == null)
            {
                return;
            }

            children.Add(define);
        }
    }

    [XmlRoot("Property")]
    public class ActorProperty
    {
        [XmlAttribute("name")]
        public string name;

        [XmlAttribute("type")]
        public VarType type;

        [XmlAttribute("desc")]
        public string desc;
    }

    [XmlRoot("Tables")]
    public class ActorTableCollection
    {
        [XmlElement("Table")]
        public List<ActorDataTable> tables = new List<ActorDataTable>();
    }

    [XmlRoot("Properties")]
    public class ActorPropertyCollection
    {
        [XmlElement("property")]
        public List<ActorProperty> properties = new List<ActorProperty>();
    }

    [XmlRoot("Table")]
    public class ActorDataTable
    {
        [XmlAttribute("WorkerName")]
        public string name;

        [XmlAttribute("desc")]
        public string desc;

        [XmlElement("Properties")]
        public List<ActorProperty> properties = new List<ActorProperty>();
    }
}