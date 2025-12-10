using System.Collections.Generic;
using Game.Scripts.Main.Runtime.UIItem.UIMenu;
using Game.Scripts.Main.Runtime.UIObject.UIMenu;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityEngine.UI;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UIDisplay.UIMenu
{
    public class ServerListScrollDisplay : ScrollDisplayBase
    {
        [SerializeField] private ServerListItem itemPrefab;

        [SerializeField] private int poolCapacity = 20;

        [SerializeField] private Text serverListDescription;

        private readonly List<ServerListItemObject> activeServerListItemObject = new();
        private readonly List<GameObject> rowGameObjects = new();
        private readonly List<int> selectedIndex = new();

        private IObjectPool<ServerListItemObject> pool;

        private void Start()
        {
            const string poolName = "ServerListItemPool";
            pool = GameEntry.ObjectPool.HasObjectPool<ServerListItemObject>(poolName)
                ? GameEntry.ObjectPool.GetObjectPool<ServerListItemObject>(poolName)
                : GameEntry.ObjectPool.CreateSingleSpawnObjectPool<ServerListItemObject>(poolName, poolCapacity, 30f,
                    16);

            Refresh();
        }

        public void Refresh()
        {
        }
    }
}