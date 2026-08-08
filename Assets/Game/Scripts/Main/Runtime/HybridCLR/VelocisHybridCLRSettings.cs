using System.Collections.Generic;

namespace Game.Scripts.Main.Runtime.HybridCLR
{
    public static class VelocisHybridCLRSettings
    {
        public static readonly bool Enable = true;

        public const string LogicMainDllName = "Velocis.HotfixMain.dll";

        public const string AssemblyTextAssetPath = "Game/HotfixDll";

        public const string HotfixNode = "Hotfix";

        public const string AotNode = "AOT";

        public const string AssemblyTextAssetExtension = ".bytes";

        public static readonly IReadOnlyList<string> HotUpdateAssemblies = new[]
        {
            "Velocis.HotfixCommon.dll",
            "Velocis.HotfixFramework.Runtime.dll",
            "Velocis.HotfixMain.dll",
            "Velocis.HotfixBusiness.dll"
        };
    }
}
