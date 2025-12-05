using Game.Scripts.Main.Runtime.UIItem.UICreate;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.UIDisplay.UICreate
{
    public class GameParameterDisplay : MonoBehaviour
    {
        [SerializeField] private Radio3Item[] items;
        public void Refresh()
        {
            items[0].SetData("Parameter.MapSize.Small", "Parameter.MapSize.Middle", "Parameter.MapSize.Big");
            items[1].SetData("Parameter.NpcCount.Small", "Parameter.NpcCount.Middle", "Parameter.NpcCount.Big");
            items[2].SetData("Parameter.SectCount.Small", "Parameter.SectCount.Middle", "Parameter.SectCount.Big");
            items[3].SetData("Parameter.FamilyCount.Small", "Parameter.FamilyCount.Middle", "Parameter.FamilyCount.Big");
        }
    }

}