using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.RuntimeException;
using Game.Scripts.Main.Runtime.SaveData;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Runtime.UIItem.UIMenu
{
    public class HeadDataItem : MonoBehaviour
    {
        [SerializeField] 
        private Text titleText;

        [SerializeField] 
        private Text dateText;

        [SerializeField] 
        private Text cultivationRealmText;

        [SerializeField] 
        private Text gameDifficultyText;

        [SerializeField] 
        private Image avatarImage;

        [SerializeField] 
        private Text createNewGame;

        public void SetData(HeadSaveData headSaveData)
        {
            SetTitle(headSaveData);
            SetDate(headSaveData);
            SetCultivationRealmText(headSaveData);
            SetGameDifficultyText(headSaveData);
            SetAvatar(headSaveData);
            HideCreateNewGame();
        }

        private void SetTitle(HeadSaveData headSaveData)
        {
            titleText.text = headSaveData.Name;
        }

        private void SetDate(HeadSaveData headSaveData)
        {
            var content = GameEntry.Localization.GetString("Date.SaveData");
            dateText.text = string.Format(content, headSaveData.Year, headSaveData.Month);
        }

        private void SetCultivationRealmText(HeadSaveData headSaveData)
        {
            var cultivationRealmType = (int)headSaveData.CultivationRealmType;
            var cultivationRealm = GameEntry.DataTable.GetDataTable<DRCultivationRealm>();
            var cultivationRealmRow = cultivationRealm.GetDataRow(cultivationRealmType);
            if (cultivationRealmRow != null)
            {
                cultivationRealmText.text = $"{GameEntry.Localization.GetString(cultivationRealmRow.Name)}{headSaveData.CultivationRealmLevel}{GameEntry.Localization.GetString("CultivationRealm.Level")}";
            }
            else
            {
                throw new GameException($"CultivationRealmType = {cultivationRealmType} not exist.");
            }
        }

        private void SetGameDifficultyText(HeadSaveData headSaveData)
        {
            var gameDifficultyType = (int)headSaveData.GameDifficultyType;
            var gameDifficulty = GameEntry.DataTable.GetDataTable<DRGameDifficulty>();
            var gameDifficultyRow = gameDifficulty.GetDataRow(gameDifficultyType);
            if (gameDifficultyRow != null)
            {
                gameDifficultyText.text = $"{GameEntry.Localization.GetString("GameDifficulty.Description")}:{GameEntry.Localization.GetString(gameDifficultyRow.Name)}";
            }
            else
            {
                throw new GameException($"GameDifficultyType = {gameDifficultyType} not exist.");
            }
        }

        private void SetAvatar(HeadSaveData headSaveData)
        {
            var avatar = GameEntry.DataTable.GetDataTable<DRAvatar>();
            var avatarRow = avatar.GetDataRow(headSaveData.Avatar);
            if (avatarRow != null)
            {
                GameEntry.Resource.LoadAsset(avatarRow.Path, typeof(Sprite), 0,
                    new LoadAssetCallbacks(
                        (assetName, asset, duration, userData) =>
                        {
                            avatarImage.sprite = asset as Sprite;
                        },
                        (assetName, asset, duration, userData) =>
                        {
                            Debug.LogError("LoadAsset " + avatarRow.Path + " error:" + duration);
                        }));
            }
            else
            {
                avatarImage.gameObject.SetActive(false);
            }
        }


        private void HideCreateNewGame()
        {
            createNewGame.gameObject.SetActive(false);
        }

        public void ReleaseAsset()
        {
            if (avatarImage.sprite == null)
            {
                return;
            }

            GameEntry.Resource.UnloadAsset(avatarImage.sprite);
            avatarImage.sprite = null;
        }
    }
}