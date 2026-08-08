using UnityEditor;

namespace Game.Scripts.Main.Editor.GeneratorAll
{
    public static class GeneratorAllMenu
    {
        [MenuItem("GeneratorAll/Generate All")]
        public static void GenerateAll()
        {
            GeneratorAllRunner.Run();
        }
    }
}
