using System.Collections.Generic;
using System.Linq;
using Celeritas.Config;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Develop
{
    [Module]
    public class VipDevelopModule : DevelopModule
    {
        private Dictionary<long, DevelopData> Items { get; } = new();

        public void ClearItems()
        {
            Items.Clear();
        }

        public void AddItem(DevelopData item, bool isLogin)
        {
            Items.Add(item.InstanceId, item);

            if (!isLogin)
            {
                GameEntry.Event.Fire(this, ChangeLevelEventArgs.Create(develop_system_type.vip));
            }
        }


        public void DeleteItem(long instanceId)
        {
            Items.Remove(instanceId);
        }


        public int GetLevel()
        {
            var firstItem = Items.Values.FirstOrDefault();
            return firstItem?.Level ?? 0;
        }
    }
}