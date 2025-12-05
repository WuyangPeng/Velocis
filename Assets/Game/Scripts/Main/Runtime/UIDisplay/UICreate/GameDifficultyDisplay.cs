using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.UIItem.UICreate;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.UIDisplay.UICreate
{
    public class GameDifficultyDisplay : MonoBehaviour
    {
        [SerializeField] 
        private GameDifficultyItem[] items;

        public void Refresh()
        {
            var gameDifficulty = GameEntry.DataTable.GetDataTable<DRGameDifficulty>();

            var gameDifficultyType = GameDifficultyType.Mortal;
            foreach (var item in items)
            {
                var data = gameDifficulty.GetDataRow((int)gameDifficultyType);
                if (data != null)
                {
                    item.SetData(data);
                }

                ++gameDifficultyType;
            }
        }
    }
}