using UnityEditor;

namespace Game.Scripts.Main.Editor.Luban
{
    public static class LubanTools
    {
        [MenuItem("Velocis/Generate Luban")]
        public static void ExportClient()
        {
            LubanExporter.Export("client");
        }
    }
}