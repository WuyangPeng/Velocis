using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.World
{
    [Module]
    public class SectModule : BaseModule
    {
        private SectData _sectData = new();

        public long GetNextSectId()
        {
            return _sectData.GetNextSectId();
        }

        public void AddSect(SectBaseData sectBaseData)
        {
            _sectData.AddSect(sectBaseData);
        }

        public SectData GetSectData()
        {
            return _sectData;
        }

        public void Init(SectData data)
        {
            _sectData = data;
        }

        public List<SectBaseData> GetSects()
        {
            return _sectData.GetSects();
        }
    }
}