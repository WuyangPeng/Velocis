using UnityEditor;

namespace Game.Scripts.Main.Editor.Luban;

public static class LubanTools
{
    [MenuItem("Velocis/Export Luban")]
    public static void ExportClient()
    {
        LubanExporter.Export("client");
    }
}