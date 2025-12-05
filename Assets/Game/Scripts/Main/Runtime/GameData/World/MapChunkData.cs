using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameData.World
{
    public class MapChunkData
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int ResourceId { get; set; }

        public int CurrentResource { get; set; }

        private HashSet<long> entity = new();

        private readonly HashSet<long> family = new();

        private readonly HashSet<long> sect = new();
        public MapChunkData(int x, int y, int resourceId)
        {
            X = x;
            Y = y;
            ResourceId = resourceId;

            var resourceTable = GameEntry.DataTable.GetDataTable<DRResource>();
            CurrentResource = resourceTable.GetDataRow(resourceId).InitValue;
        }

        public void AddFamily(long id)
        {
            family.Add(id);
        }

        public void AddSect(long id)
        {
            sect.Add(id);
        }

        public bool HasFamily(long familyId)
        {
            return family.Contains(familyId);
        }
        public bool HasEntity(long entityId)
        {
            return entity.Contains(entityId);
        }
        public void AddEntity(long playerId)
        {
            entity.Add(playerId);
        }

        public bool HasSect(long sectId)
        {
            return sect.Contains(sectId);
        }
    }
}