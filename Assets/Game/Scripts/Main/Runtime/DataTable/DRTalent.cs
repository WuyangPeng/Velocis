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
    /// 天赋表。
    /// </summary>
    public class DRTalent : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取天赋编号。
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
        /// 获取灵根id-0。
        /// </summary>
        public int SpiritualId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取灵根改变-0。
        /// </summary>
        public int SpiritualChange0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取灵根id-1。
        /// </summary>
        public int SpiritualId1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取灵根改变-1。
        /// </summary>
        public int SpiritualChange1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取功法id-0。
        /// </summary>
        public int MartialArtsId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取功法改变-0。
        /// </summary>
        public int MartialArtsChange0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取功法id-1。
        /// </summary>
        public int MartialArtsId1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取功法改变-1。
        /// </summary>
        public int MartialArtsChange1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技艺id-0。
        /// </summary>
        public int TechniqueId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技艺改变-0。
        /// </summary>
        public int TechniqueChange0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技艺id-1。
        /// </summary>
        public int TechniqueId1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技艺改变-1。
        /// </summary>
        public int TechniqueChange1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取品质。
        /// </summary>
        public int Quality
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取默认开启。
        /// </summary>
        public bool DefaultEnabled
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
            SpiritualId0 = int.Parse(columnStrings[index++]);
            SpiritualChange0 = int.Parse(columnStrings[index++]);
            SpiritualId1 = int.Parse(columnStrings[index++]);
            SpiritualChange1 = int.Parse(columnStrings[index++]);
            MartialArtsId0 = int.Parse(columnStrings[index++]);
            MartialArtsChange0 = int.Parse(columnStrings[index++]);
            MartialArtsId1 = int.Parse(columnStrings[index++]);
            MartialArtsChange1 = int.Parse(columnStrings[index++]);
            TechniqueId0 = int.Parse(columnStrings[index++]);
            TechniqueChange0 = int.Parse(columnStrings[index++]);
            TechniqueId1 = int.Parse(columnStrings[index++]);
            TechniqueChange1 = int.Parse(columnStrings[index++]);
            Quality = int.Parse(columnStrings[index++]);
            DefaultEnabled = bool.Parse(columnStrings[index++]);
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
                    SpiritualId0 = binaryReader.Read7BitEncodedInt32();
                    SpiritualChange0 = binaryReader.Read7BitEncodedInt32();
                    SpiritualId1 = binaryReader.Read7BitEncodedInt32();
                    SpiritualChange1 = binaryReader.Read7BitEncodedInt32();
                    MartialArtsId0 = binaryReader.Read7BitEncodedInt32();
                    MartialArtsChange0 = binaryReader.Read7BitEncodedInt32();
                    MartialArtsId1 = binaryReader.Read7BitEncodedInt32();
                    MartialArtsChange1 = binaryReader.Read7BitEncodedInt32();
                    TechniqueId0 = binaryReader.Read7BitEncodedInt32();
                    TechniqueChange0 = binaryReader.Read7BitEncodedInt32();
                    TechniqueId1 = binaryReader.Read7BitEncodedInt32();
                    TechniqueChange1 = binaryReader.Read7BitEncodedInt32();
                    Quality = binaryReader.Read7BitEncodedInt32();
                    DefaultEnabled = binaryReader.ReadBoolean();
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

        private KeyValuePair<int, int>[] spiritualId;

        public int SpiritualIdCount => spiritualId.Length;

        public int GetSpiritualId(int id)
        {
            foreach (var i in spiritualId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetSpiritualId with invalid id '{0}'.", id));
        }

        public int GetSpiritualIdAt(int index)
        {
            if (index < 0 || index >= spiritualId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetSpiritualIdAt with invalid index '{0}'.", index));
            }

            return spiritualId[index].Value;
        }

        private KeyValuePair<int, int>[] spiritualChange;

        public int SpiritualChangeCount => spiritualChange.Length;

        public int GetSpiritualChange(int id)
        {
            foreach (var i in spiritualChange)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetSpiritualChange with invalid id '{0}'.", id));
        }

        public int GetSpiritualChangeAt(int index)
        {
            if (index < 0 || index >= spiritualChange.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetSpiritualChangeAt with invalid index '{0}'.", index));
            }

            return spiritualChange[index].Value;
        }

        private KeyValuePair<int, int>[] martialArtsId;

        public int MartialArtsIdCount => martialArtsId.Length;

        public int GetMartialArtsId(int id)
        {
            foreach (var i in martialArtsId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetMartialArtsId with invalid id '{0}'.", id));
        }

        public int GetMartialArtsIdAt(int index)
        {
            if (index < 0 || index >= martialArtsId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetMartialArtsIdAt with invalid index '{0}'.", index));
            }

            return martialArtsId[index].Value;
        }

        private KeyValuePair<int, int>[] martialArtsChange;

        public int MartialArtsChangeCount => martialArtsChange.Length;

        public int GetMartialArtsChange(int id)
        {
            foreach (var i in martialArtsChange)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetMartialArtsChange with invalid id '{0}'.", id));
        }

        public int GetMartialArtsChangeAt(int index)
        {
            if (index < 0 || index >= martialArtsChange.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetMartialArtsChangeAt with invalid index '{0}'.", index));
            }

            return martialArtsChange[index].Value;
        }

        private KeyValuePair<int, int>[] techniqueId;

        public int TechniqueIdCount => techniqueId.Length;

        public int GetTechniqueId(int id)
        {
            foreach (var i in techniqueId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetTechniqueId with invalid id '{0}'.", id));
        }

        public int GetTechniqueIdAt(int index)
        {
            if (index < 0 || index >= techniqueId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetTechniqueIdAt with invalid index '{0}'.", index));
            }

            return techniqueId[index].Value;
        }

        private KeyValuePair<int, int>[] techniqueChange;

        public int TechniqueChangeCount => techniqueChange.Length;

        public int GetTechniqueChange(int id)
        {
            foreach (var i in techniqueChange)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetTechniqueChange with invalid id '{0}'.", id));
        }

        public int GetTechniqueChangeAt(int index)
        {
            if (index < 0 || index >= techniqueChange.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetTechniqueChangeAt with invalid index '{0}'.", index));
            }

            return techniqueChange[index].Value;
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

            spiritualId = new KeyValuePair<int, int>[]
            {
                new (0, SpiritualId0),
                new (1, SpiritualId1),
            };

            spiritualChange = new KeyValuePair<int, int>[]
            {
                new (0, SpiritualChange0),
                new (1, SpiritualChange1),
            };

            martialArtsId = new KeyValuePair<int, int>[]
            {
                new (0, MartialArtsId0),
                new (1, MartialArtsId1),
            };

            martialArtsChange = new KeyValuePair<int, int>[]
            {
                new (0, MartialArtsChange0),
                new (1, MartialArtsChange1),
            };

            techniqueId = new KeyValuePair<int, int>[]
            {
                new (0, TechniqueId0),
                new (1, TechniqueId1),
            };

            techniqueChange = new KeyValuePair<int, int>[]
            {
                new (0, TechniqueChange0),
                new (1, TechniqueChange1),
            };
        }
    }
}
