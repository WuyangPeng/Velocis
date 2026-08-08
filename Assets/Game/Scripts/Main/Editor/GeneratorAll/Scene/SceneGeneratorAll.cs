using UnityEditor;

namespace Game.Scripts.Main.Editor.GeneratorAll.Scene
{
    public static class SceneGeneratorAll
    {
        [MenuItem("GeneratorAll/Scene/Generate All")]
        public static void GenerateAll()
        {
            GeneratorAllRunner.Run("Scene");
        }
    }
}
