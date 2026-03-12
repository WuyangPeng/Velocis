using System.Collections.Generic;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;

namespace Game.Scripts.Main.Runtime.GameData.World
{
    public class MapChunkData
    {
        private readonly HashSet<long> _entity = new();

        private readonly HashSet<long> _family = new();

        private readonly HashSet<long> _sect = new();

        public MapChunkData(int x, int y, int resourceId)
        {
            X = x;
            Y = y;
            ResourceId = resourceId;

            var resourceTable = GameEntry.DataTable.GetDataTable<DRResource>();
            CurrentResource = resourceTable.GetDataRow(resourceId).InitValue;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public int ResourceId { get; set; }

        public int CurrentResource { get; set; }

        public void AddFamily(long id)
        {
            _family.Add(id);
        }

        public void AddSect(long id)
        {
            _sect.Add(id);
        }

        public bool HasFamily(long familyId)
        {
            return _family.Contains(familyId);
        }

        public bool HasEntity(long entityId)
        {
            return _entity.Contains(entityId);
        }

        public void AddEntity(long playerId)
        {
            _entity.Add(playerId);
        }

        public bool HasSect(long sectId)
        {
            return _sect.Contains(sectId);
        }
    }
}