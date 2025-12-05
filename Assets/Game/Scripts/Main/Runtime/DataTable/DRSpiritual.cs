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
    /// 灵根表。
    /// </summary>
    public class DRSpiritual : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取灵根编号。
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
        /// 获取激活值（凡品）。
        /// </summary>
        public int EnableValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取良品。
        /// </summary>
        public int QualityProduct
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取上品。
        /// </summary>
        public int TopGrade
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取极品。
        /// </summary>
        public int BestQuality
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取先天。
        /// </summary>
        public int Innate
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
            EnableValue = int.Parse(columnStrings[index++]);
            QualityProduct = int.Parse(columnStrings[index++]);
            TopGrade = int.Parse(columnStrings[index++]);
            BestQuality = int.Parse(columnStrings[index++]);
            Innate = int.Parse(columnStrings[index++]);

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
                    EnableValue = binaryReader.Read7BitEncodedInt32();
                    QualityProduct = binaryReader.Read7BitEncodedInt32();
                    TopGrade = binaryReader.Read7BitEncodedInt32();
                    BestQuality = binaryReader.Read7BitEncodedInt32();
                    Innate = binaryReader.Read7BitEncodedInt32();
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
