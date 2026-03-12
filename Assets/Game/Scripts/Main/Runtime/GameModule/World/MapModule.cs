using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.World
{
    [Module]
    public class MapModule : BaseModule
    {
        private MapData _mapData = new();

        public void AddMapChunkData(MapChunkData mapChunkData)
        {
            _mapData.AddMapChunkData(mapChunkData);
        }

        public void SetMapSize(int mapSize)
        {
            _mapData.MapSize = mapSize;
        }

        public MapData GetMapData()
        {
            return _mapData;
        }

        public void AddFamilyToRandomChunk(FamilyBaseData familyBaseData)
        {
            _mapData.AddFamilyToRandomChunk(familyBaseData);
        }

        public void AddSectToRandomChunk(SectBaseData sectBaseData)
        {
            _mapData.AddSectToRandomChunk(sectBaseData);
        }

        public void Init(MapData data)
        {
            _mapData = data;
        }

        public void SetChunkByFamilyId(long entityId, long familyId)
        {
            _mapData.SetChunkByFamilyId(entityId, familyId);
        }

        public void SetChunkBySectId(long entityId, long sectId)
        {
            _mapData.SetChunkBySectId(entityId, sectId);
        }


        public bool HasEntity(long entityId)
        {
            return _mapData.HasEntity(entityId);
        }

        public void AddEntityToRandomChunk(long entityId)
        {
            _mapData.AddEntityToRandomChunk(entityId);
        }
    }
}