using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Main.Editor.Protobuf
{
    public sealed class CeleritasGenerator : EditorWindow
    {
        [MenuItem("Velocis/Generate Protobuf Celeritas")]
        public static void Generate()
        {
            var generator = new CeleritasGeneratorInstance();
            generator.Run();
            Debug.Log("CSCeleritas generated successfully.");
        }
    }
}