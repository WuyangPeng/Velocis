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
    /// 难度表。
    /// </summary>
    public class DRGameDifficulty : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取难度编号。
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
        /// 获取世界描述。
        /// </summary>
        public string WorldDescription
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取敌人描述。
        /// </summary>
        public string EnemyDescription
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取队伍描述。
        /// </summary>
        public string TeamDescription
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取资源等级。
        /// </summary>
        public int ResourceLevel
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
            WorldDescription = columnStrings[index++];
            EnemyDescription = columnStrings[index++];
            TeamDescription = columnStrings[index++];
            ResourceLevel = int.Parse(columnStrings[index++]);

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
                    WorldDescription = binaryReader.ReadString();
                    EnemyDescription = binaryReader.ReadString();
                    TeamDescription = binaryReader.ReadString();
                    ResourceLevel = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
