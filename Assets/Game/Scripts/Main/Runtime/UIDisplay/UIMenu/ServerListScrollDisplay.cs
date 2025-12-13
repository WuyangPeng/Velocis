using System;
using System.Collections.Generic;
using System.Net;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Login;
using Game.Scripts.Main.Runtime.Network;
using Game.Scripts.Main.Runtime.UIItem.UIMenu;
using Game.Scripts.Main.Runtime.UIObject.UIMenu;
using GameFramework.Network;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;
using AddressFamily = System.Net.Sockets.AddressFamily;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UIDisplay.UIMenu
{
    public class ServerListScrollDisplay : ScrollDisplayBase
    {
        [SerializeField] private ServerListItem itemPrefab;

        [SerializeField] private int poolCapacity = 20;

        private readonly List<ServerListItemObject> activeServerListItemObject = new();
        private readonly List<GameObject> rowGameObjects = new();
        private IObjectPool<ServerListItemObject> pool;

        private void Start()
        {
            const string poolName = "ServerListItemPool";
            pool = GameEntry.ObjectPool.HasObjectPool<ServerListItemObject>(poolName)
                ? GameEntry.ObjectPool.GetObjectPool<ServerListItemObject>(poolName)
                : GameEntry.ObjectPool.CreateSingleSpawnObjectPool<ServerListItemObject>(poolName, poolCapacity, 30f,
                    16);

            UnSpawn();
            SetData();
        }

        private void SetData()
        {
            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            var loginServerInfo = accountModule.GetLoginServerInfo();

            for (var row = 0; row < loginServerInfo.Count; row++)
            {
                if (!SetData(loginServerInfo[row], row))
                {
                    Log.Warning("login server list set data error.row = " + row);
                }
            }
        }

        private bool SetData(LoginServerInfo loginServerInfo, int row)
        {
            var rowGameObject = GetRowGameObject(row, TextAnchor.LowerCenter, 70);

            rowGameObjects.Add(rowGameObject);

            return SetData(loginServerInfo, row, rowGameObject);
        }

        private bool SetData(LoginServerInfo loginServerInfo, int row, GameObject rowGameObject)
        {
            var spawn = GetSpawn();
            if (spawn == null)
            {
                return false;
            }

            activeServerListItemObject.Add(spawn);

            var avatarItem = (ServerListItem)spawn.Target;
            avatarItem.transform.SetParent(rowGameObject.transform, false);
            avatarItem.SetData(row, loginServerInfo, OnItemClick);

            return true;
        }

        private ServerListItemObject GetSpawn()
        {
            var result = pool.Spawn();
            if (result != null)
            {
                return result;
            }

            var itemGameObject = Instantiate(itemPrefab.gameObject, null);
            if (itemGameObject.TryGetComponent<ServerListItem>(out var item))
            {
                var avatarItemObject = ServerListItemObject.Create(item);
                pool.Register(avatarItemObject, true);
                pool.Unspawn(avatarItemObject);
                result = pool.Spawn();

                return result;
            }

            Log.Error("预制体没挂 ServerListItem");
            Destroy(itemGameObject);
            return null;
        }

        private void UnSpawn()
        {
            foreach (var element in activeServerListItemObject)
            {
                var item = (ServerListItem)element.Target;
                if (item != null && item.gameObject != null)
                {
                    item.transform.SetParent(null, false);
                }

                pool.Unspawn(element);
            }

            activeServerListItemObject.Clear();

            foreach (var rowGameObject in rowGameObjects) DestroyImmediate(rowGameObject);

            rowGameObjects.Clear();
        }

        private void OnItemClick(int index)
        {
            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            var currentLoginServerInfo = accountModule.SetCurrentLoginServerInfo(index);
            var host = currentLoginServerInfo.connection_info.host;
            var port = currentLoginServerInfo.connection_info.port;

            IPAddress ipAddress;
            try
            {
                if (!IPAddress.TryParse(host, out ipAddress))
                {
                    var addresses = Dns.GetHostAddresses(host);
                    if (addresses.Length == 0)
                    {
                        Log.Error("Unable to resolve host '{0}'.", host);
                        return;
                    }

                    ipAddress = Array.Find(addresses, a => a.AddressFamily == AddressFamily.InterNetwork) ??
                                addresses[0];
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to resolve host '{0}': {1}", host, ex.Message);
                return;
            }


            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel") ??
                          GameEntry.Network.CreateNetworkChannel("TcpChannel", ServiceType.Tcp,
                              new NetworkChannelHelper());

            channel.Connect(ipAddress, port);
        }
    }
}