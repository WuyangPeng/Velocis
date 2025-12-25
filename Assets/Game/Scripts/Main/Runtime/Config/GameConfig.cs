using System.Collections.Generic;
using System.IO;
using Celeritas.Config;
using Game.Scripts.Main.Runtime.GameUtility;
using Game.Scripts.Main.Runtime.RuntimeException;
using GameFramework.Resource;
using Luban;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Config;

public class GameConfig
{
    private readonly Dictionary<string, ByteBuf> _byteBuf = new();
    private int _loadTablesSize;
    private tables _tables;
    private int _tablesSize;

    public tables GetTables()
    {
        return _tables;
    }

    public void Initialize()
    {
        _byteBuf.Clear();
        _loadTablesSize = 0;
        LoadByteBuf();
    }

    private void LoadByteBuf()
    {
        var files = Directory.GetFiles(Path.Combine(Application.dataPath, "Game/Bin"), "*.bytes");
        _tablesSize = files.Length;
        foreach (var file in files)
        {
            GameEntry.Resource.LoadBinary(AssetUtility.GetLubanAsset(Path.GetFileNameWithoutExtension(file)), new LoadBinaryCallbacks(OnLoadBinarySuccess, OnLoadBinaryFailure));
        }
    }

    private ByteBuf LoadByteBuf(string file)
    {
        return _byteBuf.TryGetValue(AssetUtility.GetLubanAsset(file), out var buf) ? buf : throw new GameException($"error byte,file = {file}");
    }

    private void OnLoadBinarySuccess(string binaryAssetName, byte[] binaryBytes, float duration, object userData)
    {
        _byteBuf.Add(binaryAssetName, new ByteBuf(binaryBytes));
        Log.Info("load binary success,file = {0}", binaryAssetName);
        ++_loadTablesSize;
        if (_loadTablesSize != _tablesSize)
        {
            return;
        }

        _tables = new tables(LoadByteBuf);
        _byteBuf.Clear();
    }

    private void OnLoadBinaryFailure(string binaryAssetName, LoadResourceStatus status, string errorMessage, object userData)
    {
        _byteBuf.Clear();
        throw new GameException($"load binary failure,file = {binaryAssetName}");
    }
}