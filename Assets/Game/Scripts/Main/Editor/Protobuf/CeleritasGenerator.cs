using UnityEditor;

namespace Game.Scripts.Main.Editor.Protobuf
{
    public sealed class CeleritasGenerator : EditorWindow
    {
        [MenuItem("Velocis/Generate Protobuf Celeritas")]
        public static void Generate()
        {
            var generator = new CsCeleritasGeneratorInstance();
            generator.Run();
        }
    }
}