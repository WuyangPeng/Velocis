using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Runtime.UIItem.UICreate
{
    public class GameDifficultyItem : MonoBehaviour
    {
        [SerializeField] 
        private Text gameDifficultyText;

        [SerializeField] 
        private Text worldText;

        [SerializeField] 
        private Text enemyText;

        [SerializeField] 
        private Text teamText;

        public void SetData(DRGameDifficulty data)
        {
            gameDifficultyText.text = GameEntry.Localization.GetString(data.Name);
            worldText.text = GameEntry.Localization.GetString(data.WorldDescription);
            enemyText.text = GameEntry.Localization.GetString(data.EnemyDescription);
            teamText.text = GameEntry.Localization.GetString(data.TeamDescription);
        }
    }
}