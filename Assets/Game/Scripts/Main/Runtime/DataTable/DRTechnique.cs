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
    /// 技艺表。
    /// </summary>
    public class DRTechnique : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取功法编号。
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
        /// 获取初始值。
        /// </summary>
        public int InitValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最大值。
        /// </summary>
        public int MaxValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取激活值（入门）。
        /// </summary>
        public int Beginner
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取精通。
        /// </summary>
        public int Proficient
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取大师。
        /// </summary>
        public int Master
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取宗师。
        /// </summary>
        public int Grandmaster
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取传说。
        /// </summary>
        public int Legendary
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
            InitValue = int.Parse(columnStrings[index++]);
            MaxValue = int.Parse(columnStrings[index++]);
            Beginner = int.Parse(columnStrings[index++]);
            Proficient = int.Parse(columnStrings[index++]);
            Master = int.Parse(columnStrings[index++]);
            Grandmaster = int.Parse(columnStrings[index++]);
            Legendary = int.Parse(columnStrings[index++]);

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
                    InitValue = binaryReader.Read7BitEncodedInt32();
                    MaxValue = binaryReader.Read7BitEncodedInt32();
                    Beginner = binaryReader.Read7BitEncodedInt32();
                    Proficient = binaryReader.Read7BitEncodedInt32();
                    Master = binaryReader.Read7BitEncodedInt32();
                    Grandmaster = binaryReader.Read7BitEncodedInt32();
                    Legendary = binaryReader.Read7BitEncodedInt32();
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
