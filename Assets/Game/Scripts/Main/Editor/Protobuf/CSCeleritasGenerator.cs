using UnityEditor;

namespace Game.Scripts.Main.Editor.Protobuf
{
    public sealed class CSCeleritasGenerator : EditorWindow
    {
        [MenuItem("Velocis/Generate Protobuf CSCeleritas")]
        public static void Generate()
        {
            var generator = new CsCeleritasGeneratorInstance();
            generator.Run();
        }
    }
}