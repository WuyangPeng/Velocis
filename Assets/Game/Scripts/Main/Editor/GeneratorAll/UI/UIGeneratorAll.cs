using UnityEditor;

namespace Game.Scripts.Main.Editor.GeneratorAll.UI
{
    public static class UIGeneratorAll
    {
        [MenuItem("GeneratorAll/UI/Generate All")]
        public static void GenerateAll()
        {
            GeneratorAllRunner.Run("UI");
        }
    }
}
