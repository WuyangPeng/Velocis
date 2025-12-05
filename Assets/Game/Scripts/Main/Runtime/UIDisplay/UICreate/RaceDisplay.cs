using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.UIItem.UICreate;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.UIDisplay.UICreate
{
    public class RaceDisplay : MonoBehaviour
    {
        [SerializeField] 
        private Radio2Item items;
        public void Refresh()
        {
            var race = GameEntry.DataTable.GetDataTable<DRRace>();

            items.SetData(race.GetDataRow((int)RaceType.Human).Name, race.GetDataRow((int)RaceType.Demon).Name);
        }
    }
}