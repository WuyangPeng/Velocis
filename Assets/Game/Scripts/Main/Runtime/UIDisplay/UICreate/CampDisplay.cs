using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.UIItem.UICreate;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.UIDisplay.UICreate
{
    public class CampDisplay : MonoBehaviour
    {
        [SerializeField]
        private Radio3Item[] items;
        public void Refresh()
        {
            var camp = GameEntry.DataTable.GetDataTable<DRCamp>();

            items[0].SetData(camp.GetDataRow((int)RulesType.Lawful).Name,
                camp.GetDataRow((int)RulesType.Carefree).Name,
                camp.GetDataRow((int)RulesType.Chaos).Name);
            items[1].SetData(camp.GetDataRow((int)MoralityType.Benevolence).Name,
                camp.GetDataRow((int)MoralityType.Moderation).Name,
                camp.GetDataRow((int)MoralityType.Craftiness).Name);
        }
    }
}