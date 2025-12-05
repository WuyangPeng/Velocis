//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.DataTable
{
    /// <summary>
    /// 种族表。
    /// </summary>
    public class DRRace : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取种族编号。
        /// </summary>
        public override int Id => m_Id;

        /// <summary>
        /// 获取名字。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取属性id-0。
        /// </summary>
        public int PropertyId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取属性改变-0。
        /// </summary>
        public int PropertyChange0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取属性id-1。
        /// </summary>
        public int PropertyId1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取属性改变-1。
        /// </summary>
        public int PropertyChange1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取权重。
        /// </summary>
        public float Weight
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            var columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (var i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            var index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            Name = columnStrings[index++];
            Description = columnStrings[index++];
            PropertyId0 = int.Parse(columnStrings[index++]);
            PropertyChange0 = int.Parse(columnStrings[index++]);
            PropertyId1 = int.Parse(columnStrings[index++]);
            PropertyChange1 = int.Parse(columnStrings[index++]);
            Weight = float.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (var memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (var binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Name = binaryReader.ReadString();
                    Description = binaryReader.ReadString();
                    PropertyId0 = binaryReader.Read7BitEncodedInt32();
                    PropertyChange0 = binaryReader.Read7BitEncodedInt32();
                    PropertyId1 = binaryReader.Read7BitEncodedInt32();
                    PropertyChange1 = binaryReader.Read7BitEncodedInt32();
                    Weight = binaryReader.ReadSingle();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, int>[] propertyId;

        public int PropertyIdCount => propertyId.Length;

        public int GetPropertyId(int id)
        {
            foreach (var i in propertyId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetPropertyId with invalid id '{0}'.", id));
        }

        public int GetPropertyIdAt(int index)
        {
            if (index < 0 || index >= propertyId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetPropertyIdAt with invalid index '{0}'.", index));
            }

            return propertyId[index].Value;
        }

        private KeyValuePair<int, int>[] propertyChange;

        public int PropertyChangeCount => propertyChange.Length;

        public int GetPropertyChange(int id)
        {
            foreach (var i in propertyChange)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetPropertyChange with invalid id '{0}'.", id));
        }

        public int GetPropertyChangeAt(int index)
        {
            if (index < 0 || index >= propertyChange.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetPropertyChangeAt with invalid index '{0}'.", index));
            }

            return propertyChange[index].Value;
        }

        private void GeneratePropertyArray()
        {
            propertyId = new KeyValuePair<int, int>[]
            {
                new (0, PropertyId0),
                new (1, PropertyId1),
            };

            propertyChange = new KeyValuePair<int, int>[]
            {
                new (0, PropertyChange0),
                new (1, PropertyChange1),
            };
        }
    }
}
