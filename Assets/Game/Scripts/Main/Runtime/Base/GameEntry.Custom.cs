using Game.Scripts.Main.Runtime.Account;
using Game.Scripts.Main.Runtime.BuiltinData;
using Game.Scripts.Main.Runtime.HPBar;
using Game.Scripts.Main.Runtime.Network;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Base
{
    /// <summary>
    ///     游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static BuiltinDataComponent BuiltinData { get; private set; }

        public static HpBarComponent HpBar { get; private set; }

        public static AccountComponent Account { get; private set; }


        public static CeleritasHandlerComponent CeleritasHandler { get; private set; }

        private static void InitCustomComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            HpBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HpBarComponent>();
            Account = UnityGameFramework.Runtime.GameEntry.GetComponent<AccountComponent>();
            CeleritasHandler = UnityGameFramework.Runtime.GameEntry.GetComponent<CeleritasHandlerComponent>();
        }
    }
}