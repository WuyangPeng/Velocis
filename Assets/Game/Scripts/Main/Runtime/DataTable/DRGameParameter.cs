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
    /// 游戏参数表。
    /// </summary>
    public class DRGameParameter : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取游戏参数编号。
        /// </summary>
        public override int Id => m_Id;

        /// <summary>
        /// 获取最小地图大小。
        /// </summary>
        public int MinMapSize
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最大地图大小。
        /// </summary>
        public int MaxMapSize
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最小NPC数量。
        /// </summary>
        public int MinNpcCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最大NPC数量。
        /// </summary>
        public int MaxNpcCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最小宗门数量。
        /// </summary>
        public int MinSectCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最大宗门数量。
        /// </summary>
        public int MaxSectCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最小家族数量。
        /// </summary>
        public int MinFamilyCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最大家族数量。
        /// </summary>
        public int MaxFamilyCount
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
            MinMapSize = int.Parse(columnStrings[index++]);
            MaxMapSize = int.Parse(columnStrings[index++]);
            MinNpcCount = int.Parse(columnStrings[index++]);
            MaxNpcCount = int.Parse(columnStrings[index++]);
            MinSectCount = int.Parse(columnStrings[index++]);
            MaxSectCount = int.Parse(columnStrings[index++]);
            MinFamilyCount = int.Parse(columnStrings[index++]);
            MaxFamilyCount = int.Parse(columnStrings[index++]);

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
                    MinMapSize = binaryReader.Read7BitEncodedInt32();
                    MaxMapSize = binaryReader.Read7BitEncodedInt32();
                    MinNpcCount = binaryReader.Read7BitEncodedInt32();
                    MaxNpcCount = binaryReader.Read7BitEncodedInt32();
                    MinSectCount = binaryReader.Read7BitEncodedInt32();
                    MaxSectCount = binaryReader.Read7BitEncodedInt32();
                    MinFamilyCount = binaryReader.Read7BitEncodedInt32();
                    MaxFamilyCount = binaryReader.Read7BitEncodedInt32();
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
