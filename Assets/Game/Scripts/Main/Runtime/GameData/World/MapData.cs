using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameData.World
{
    public class MapData
    {
        private readonly Dictionary<long, int> _entity = new();
        private readonly Dictionary<long, int> _family = new();
        private readonly List<MapChunkData> _mapChunkContainer = new();
        private readonly Dictionary<long, int> _sect = new();
        public int MapSize { get; set; }

        public void AddMapChunkData(MapChunkData mapChunkData)
        {
            _mapChunkContainer.Add(mapChunkData);
        }

        public void AddFamilyToRandomChunk(FamilyBaseData familyBaseData)
        {
            var index = Random.Range(0, _mapChunkContainer.Count);
            _family.Add(familyBaseData.ID, index);
            _mapChunkContainer[index].AddFamily(familyBaseData.ID);
        }

        public void AddSectToRandomChunk(SectBaseData sectBaseData)
        {
            var index = Random.Range(0, _mapChunkContainer.Count);
            _sect.Add(sectBaseData.ID, index);
            _mapChunkContainer[index].AddSect(sectBaseData.ID);
        }

        public void SetChunkByFamilyId(long entityId, long familyId)
        {
            if (!_family.TryGetValue(familyId, out var index))
            {
                return;
            }

            var mapChunkData = _mapChunkContainer[index];
            mapChunkData.AddEntity(entityId);
        }

        public void SetChunkBySectId(long entityId, long sectId)
        {
            if (!_sect.TryGetValue(sectId, out var index))
            {
                return;
            }

            var mapChunkData = _mapChunkContainer[index];
            mapChunkData.AddEntity(entityId);
        }

        public bool HasEntity(long entityId)
        {
            return _entity.ContainsKey(entityId);
        }

        public void AddEntityToRandomChunk(long entityId)
        {
            var index = Random.Range(0, _mapChunkContainer.Count);
            _entity.Add(entityId, index);
            _mapChunkContainer[index].AddEntity(entityId);
        }
    }
}