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
    /// 战机表。
    /// </summary>
    public class DRAircraft : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取战机编号。
        /// </summary>
        public override int Id => m_Id;

        /// <summary>
        /// 获取推进器编号。
        /// </summary>
        public int ThrusterId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取武器编号0。
        /// </summary>
        public int WeaponId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取武器编号1。
        /// </summary>
        public int WeaponId1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取武器编号2。
        /// </summary>
        public int WeaponId2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取装甲编号0。
        /// </summary>
        public int ArmorId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取装甲编号1。
        /// </summary>
        public int ArmorId1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取装甲编号2。
        /// </summary>
        public int ArmorId2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡特效编号。
        /// </summary>
        public int DeadEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡声音编号。
        /// </summary>
        public int DeadSoundId
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
            ThrusterId = int.Parse(columnStrings[index++]);
            WeaponId0 = int.Parse(columnStrings[index++]);
            WeaponId1 = int.Parse(columnStrings[index++]);
            WeaponId2 = int.Parse(columnStrings[index++]);
            ArmorId0 = int.Parse(columnStrings[index++]);
            ArmorId1 = int.Parse(columnStrings[index++]);
            ArmorId2 = int.Parse(columnStrings[index++]);
            DeadEffectId = int.Parse(columnStrings[index++]);
            DeadSoundId = int.Parse(columnStrings[index++]);

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
                    ThrusterId = binaryReader.Read7BitEncodedInt32();
                    WeaponId0 = binaryReader.Read7BitEncodedInt32();
                    WeaponId1 = binaryReader.Read7BitEncodedInt32();
                    WeaponId2 = binaryReader.Read7BitEncodedInt32();
                    ArmorId0 = binaryReader.Read7BitEncodedInt32();
                    ArmorId1 = binaryReader.Read7BitEncodedInt32();
                    ArmorId2 = binaryReader.Read7BitEncodedInt32();
                    DeadEffectId = binaryReader.Read7BitEncodedInt32();
                    DeadSoundId = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, int>[] weaponId;

        public int WeaponIdCount => weaponId.Length;

        public int GetWeaponId(int id)
        {
            foreach (var i in weaponId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetWeaponId with invalid id '{0}'.", id));
        }

        public int GetWeaponIdAt(int index)
        {
            if (index < 0 || index >= weaponId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetWeaponIdAt with invalid index '{0}'.", index));
            }

            return weaponId[index].Value;
        }

        private KeyValuePair<int, int>[] armorId;

        public int ArmorIdCount => armorId.Length;

        public int GetArmorId(int id)
        {
            foreach (var i in armorId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetArmorId with invalid id '{0}'.", id));
        }

        public int GetArmorIdAt(int index)
        {
            if (index < 0 || index >= armorId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetArmorIdAt with invalid index '{0}'.", index));
            }

            return armorId[index].Value;
        }

        private void GeneratePropertyArray()
        {
            weaponId = new KeyValuePair<int, int>[]
            {
                new (0, WeaponId0),
                new (1, WeaponId1),
                new (2, WeaponId2),
            };

            armorId = new KeyValuePair<int, int>[]
            {
                new (0, ArmorId0),
                new (1, ArmorId1),
                new (2, ArmorId2),
            };
        }
    }
}
