using UnityEditor;

namespace Game.Scripts.Main.Editor.GeneratorAll.UI.Component
{
    public static class ComponentGeneratorAll
    {
        [MenuItem("GeneratorAll/UI/Component/Generate All")]
        public static void GenerateAll()
        {
            GeneratorAllRunner.Run("UI/Component");
        }
    }
}
