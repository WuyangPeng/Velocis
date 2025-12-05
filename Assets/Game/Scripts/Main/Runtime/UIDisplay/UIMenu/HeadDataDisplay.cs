using System.Collections.Generic;
using Game.Scripts.Main.Runtime.SaveData;
using Game.Scripts.Main.Runtime.UIItem.UIMenu;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.UIDisplay.UIMenu
{
    public class HeadDataDisplay : MonoBehaviour
    {
        [SerializeField] 
        private HeadDataItem[] items;

        public void Refresh(List<HeadSaveData> headData)
        {
            foreach (var data in headData)
            {
                items[data.Index].SetData(data);
                items[data.Index].gameObject.SetActive(true);
            }
        }

        public void ReleaseAsset()
        {
            foreach (var data in items)
            {
                data.ReleaseAsset();
            }
        }
    }
}