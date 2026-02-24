using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Develop
{
    [Module]
    public class RoleDevelopModule : DevelopModule
    {
        private Dictionary<long, DevelopData> Items { get; } = new();

        public void ClearItems()
        {
            Items.Clear();
        }

        public void AddItem(DevelopData item)
        {
            Items.Add(item.InstanceId, item);
            
            GameEntry.Event.Fire(this, ChangeLevelEventArgs.Create());
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