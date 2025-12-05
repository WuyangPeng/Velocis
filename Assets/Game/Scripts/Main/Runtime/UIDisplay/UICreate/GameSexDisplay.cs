using Game.Scripts.Main.Runtime.UIItem.UICreate;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.UIDisplay.UICreate
{
    public class GameSexDisplay : MonoBehaviour
    {
        [SerializeField] private Radio2Item radio2Item;
        public void Refresh()
        {
            radio2Item.SetData("Sex.Male", "Sex.Female");
        }
    }
}