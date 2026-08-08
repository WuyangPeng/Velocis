using UnityEditor;

namespace Game.Scripts.Main.Editor.GeneratorAll.UI.Form
{
    public static class FormGeneratorAll
    {
        [MenuItem("GeneratorAll/UI/Form/Generate All")]
        public static void GenerateAll()
        {
            // Form 依赖 Component 预制体，先跑同级 Component 再跑 Form。
            GeneratorAllRunner.Run("UI/Component", "UI/Form");
        }
    }
}
