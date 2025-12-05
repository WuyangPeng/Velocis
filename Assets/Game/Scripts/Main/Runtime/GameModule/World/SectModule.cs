using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameModule.Base;
using System.Collections.Generic;

namespace Game.Scripts.Main.Runtime.GameModule.World
{
    [Module]
    public class SectModule : BaseModule
    {
        private SectData sectData = new();

        public long GetNextSectId()
        {
            return sectData.GetNextSectId();
        }

        public void AddSect(SectBaseData sectBaseData)
        {
            sectData.AddSect(sectBaseData);
        }

        public SectData GetSectData()
        {
            return sectData;
        }

        public void Init(SectData data)
        {
            sectData = data;
        }

        public List<SectBaseData> GetSects()
        {
            return sectData.GetSects();
        }
    }
}