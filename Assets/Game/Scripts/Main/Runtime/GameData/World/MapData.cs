using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameData.World
{
    public class MapData
    {
        public int MapSize { get; set; }
        private readonly List<MapChunkData> mapChunkContainer = new();
        private readonly Dictionary<long, int> family = new();
        private readonly Dictionary<long, int> sect = new();
        private readonly Dictionary<long, int> entity = new();

        public void AddMapChunkData(MapChunkData mapChunkData)
        {
            mapChunkContainer.Add(mapChunkData);
        }

        public void AddFamilyToRandomChunk(FamilyBaseData familyBaseData)
        {
            var index = Random.Range(0, mapChunkContainer.Count);
            family.Add(familyBaseData.ID, index);
            mapChunkContainer[index].AddFamily(familyBaseData.ID);
        }

        public void AddSectToRandomChunk(SectBaseData sectBaseData)
        {
            var index = Random.Range(0, mapChunkContainer.Count);
            sect.Add(sectBaseData.ID, index);
            mapChunkContainer[index].AddSect(sectBaseData.ID);
        }

        public void SetChunkByFamilyId(long entityId, long familyId)
        {
            if (!family.TryGetValue(familyId, out var index))
            {
                return;
            }

            var mapChunkData = mapChunkContainer[index];
            mapChunkData.AddEntity(entityId);
        }

        public void SetChunkBySectId(long entityId, long sectId)
        {
            if (!sect.TryGetValue(sectId, out var index))
            {
                return;
            }

            var mapChunkData = mapChunkContainer[index];
            mapChunkData.AddEntity(entityId);
        }

        public bool HasEntity(long entityId)
        {
            return entity.ContainsKey(entityId);
        }

        public void AddEntityToRandomChunk(long entityId)
        {
            var index = Random.Range(0, mapChunkContainer.Count);
            entity.Add(entityId, index);
            mapChunkContainer[index].AddEntity(entityId);
        }
    }
}