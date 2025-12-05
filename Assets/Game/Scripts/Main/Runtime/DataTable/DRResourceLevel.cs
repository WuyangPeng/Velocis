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
    /// 资源等级表。
    /// </summary>
    public class DRResourceLevel : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取资源等级编号。
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
        /// 获取级别0。
        /// </summary>
        public float Level0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取级别1。
        /// </summary>
        public float Level1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取级别2。
        /// </summary>
        public float Level2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取级别3。
        /// </summary>
        public float Level3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取级别4。
        /// </summary>
        public float Level4
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取级别5。
        /// </summary>
        public float Level5
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取级别6。
        /// </summary>
        public float Level6
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取级别7。
        /// </summary>
        public float Level7
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取级别8。
        /// </summary>
        public float Level8
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取级别9。
        /// </summary>
        public float Level9
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
            Level0 = float.Parse(columnStrings[index++]);
            Level1 = float.Parse(columnStrings[index++]);
            Level2 = float.Parse(columnStrings[index++]);
            Level3 = float.Parse(columnStrings[index++]);
            Level4 = float.Parse(columnStrings[index++]);
            Level5 = float.Parse(columnStrings[index++]);
            Level6 = float.Parse(columnStrings[index++]);
            Level7 = float.Parse(columnStrings[index++]);
            Level8 = float.Parse(columnStrings[index++]);
            Level9 = float.Parse(columnStrings[index++]);

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
                    Level0 = binaryReader.ReadSingle();
                    Level1 = binaryReader.ReadSingle();
                    Level2 = binaryReader.ReadSingle();
                    Level3 = binaryReader.ReadSingle();
                    Level4 = binaryReader.ReadSingle();
                    Level5 = binaryReader.ReadSingle();
                    Level6 = binaryReader.ReadSingle();
                    Level7 = binaryReader.ReadSingle();
                    Level8 = binaryReader.ReadSingle();
                    Level9 = binaryReader.ReadSingle();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, float>[] level;

        public int LevelCount => level.Length;

        public float GetLevel(int id)
        {
            foreach (var i in level)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetLevel with invalid id '{0}'.", id));
        }

        public float GetLevelAt(int index)
        {
            if (index < 0 || index >= level.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetLevelAt with invalid index '{0}'.", index));
            }

            return level[index].Value;
        }

        private void GeneratePropertyArray()
        {
            level = new KeyValuePair<int, float>[]
            {
                new (0, Level0),
                new (1, Level1),
                new (2, Level2),
                new (3, Level3),
                new (4, Level4),
                new (5, Level5),
                new (6, Level6),
                new (7, Level7),
                new (8, Level8),
                new (9, Level9),
            };
        }
    }
}
