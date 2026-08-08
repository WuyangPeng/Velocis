using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Game.Scripts.Main.Runtime.HybridCLR;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.Procedure
{
    public class ProcedureLoadAssembly : ProcedureBase
    {
        private static readonly List<string> HotfixDlls = new()
        {
            "Velocis.HotfixCommon.dll",
            "Velocis.HotfixFramework.Runtime.dll",
            "Velocis.HotfixMain.dll",
            "Velocis.HotfixBusiness.dll"
        };

        private LoadAssetCallbacks _loadAssetCallbacks;
        private int _loadAssetCount;
        private int _loadAssemblyWait;
        private bool _loadAssemblyComplete;
        private bool _hasEnter;
        private Assembly _mainLogicAssembly;
        private List<Assembly> _hotfixAssemblies;
        private Dictionary<string, byte[]> _hotfixAssemblyBytes;

        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _loadAssemblyComplete = false;
            _hasEnter = false;
            _hotfixAssemblies = new List<Assembly>();
            _hotfixAssemblyBytes = new Dictionary<string, byte[]>();

            Log.Info("HybridCLR enabled: {0}", VelocisHybridCLRSettings.Enable);

            if (HotfixDlls.Count != VelocisHybridCLRSettings.HotUpdateAssemblies.Count)
            {
                Log.Error("Hotfix DLL configuration is invalid.");
                return;
            }

            if (GameEntry.Base.EditorResourceMode)
            {
                _mainLogicAssembly = GetMainLogicAssembly();
            }
            else
            {
#if UNITY_EDITOR
                _mainLogicAssembly = GetMainLogicAssembly();
#else
                if (VelocisHybridCLRSettings.Enable)
                {
                    _loadAssetCallbacks ??= new LoadAssetCallbacks(LoadAssetSuccess, LoadAssetFailure);
                    foreach (var hotUpdateDllName in VelocisHybridCLRSettings.HotUpdateAssemblies)
                    {
                        var assetPath = Utility.Path.GetRegularPath(Path.Combine(
                            "Assets",
                            VelocisHybridCLRSettings.AssemblyTextAssetPath,
                            VelocisHybridCLRSettings.HotfixNode,
                            $"{hotUpdateDllName}{VelocisHybridCLRSettings.AssemblyTextAssetExtension}"));
                        Log.Debug("Load hotfix asset: {0}", assetPath);
                        _loadAssetCount++;
                        GameEntry.Resource.LoadAsset(assetPath, _loadAssetCallbacks, hotUpdateDllName);
                    }

                    _loadAssemblyWait = 1;
                }
                else
                {
                    _mainLogicAssembly = GetMainLogicAssembly();
                }
#endif
            }

            if (_loadAssetCount == 0)
            {
                _loadAssemblyComplete = true;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (_hasEnter || !_loadAssemblyComplete)
            {
                return;
            }

            AllAsmLoadComplete();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.UI.CloseAllLoadingUIForms();
        }

        private Assembly GetMainLogicAssembly()
        {
            Assembly mainLogicAssembly = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (string.Compare(
                        VelocisHybridCLRSettings.LogicMainDllName,
                        $"{asm.GetName().Name}.dll",
                        StringComparison.Ordinal) == 0)
                {
                    mainLogicAssembly = asm;
                }

                foreach (var hotUpdateDllName in VelocisHybridCLRSettings.HotUpdateAssemblies)
                {
                    if (hotUpdateDllName == $"{asm.GetName().Name}.dll")
                    {
                        _hotfixAssemblies.Add(asm);
                    }
                }

                if (mainLogicAssembly != null &&
                    _hotfixAssemblies.Count == VelocisHybridCLRSettings.HotUpdateAssemblies.Count)
                {
                    break;
                }
            }

            return mainLogicAssembly;
        }

        private void LoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            _loadAssetCount--;
            Log.Debug("Load hotfix asset success: {0}", assetName);

            if (asset is not TextAsset textAsset)
            {
                Log.Error("Load hotfix asset '{0}' failed.", assetName);
                return;
            }

            var dllName = userData as string;
            _hotfixAssemblyBytes[dllName] = textAsset.bytes;

            if (_hotfixAssemblyBytes.Count != VelocisHybridCLRSettings.HotUpdateAssemblies.Count)
            {
                return;
            }

            foreach (var hotfixDllName in HotfixDlls)
            {
                var asm = Assembly.Load(_hotfixAssemblyBytes[hotfixDllName]);
                if (string.Compare(
                        VelocisHybridCLRSettings.LogicMainDllName,
                        hotfixDllName,
                        StringComparison.Ordinal) == 0)
                {
                    _mainLogicAssembly = asm;
                }

                _hotfixAssemblies.Add(asm);
                Log.Debug("Assembly '{0}' loaded.", asm.GetName().Name);
            }

            _loadAssemblyComplete = _loadAssemblyWait != 0 && _loadAssetCount == 0;
        }

        private void LoadAssetFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            _loadAssetCount--;
            Log.Error(
                "Load hotfix asset failure, assetName: '{0}', status: '{1}', error: '{2}'.",
                assetName,
                status,
                errorMessage);
        }

        private void AllAsmLoadComplete()
        {
            if (_mainLogicAssembly == null)
            {
                Log.Fatal("Main logic assembly is missing.");
                return;
            }

            var appType = _mainLogicAssembly.GetType("Game.Scripts.Hotfix.HotfixMain.AppMain");
            if (appType == null)
            {
                Log.Fatal("Main logic type 'AppMain' is missing.");
                return;
            }

            var entryMethod = appType.GetMethod("Entrance");
            if (entryMethod == null)
            {
                Log.Fatal("Main logic entry method 'Entrance' is missing.");
                return;
            }

            _hasEnter = true;
            entryMethod.Invoke(appType, new object[] { new object[] { _hotfixAssemblies } });
        }
    }
}
