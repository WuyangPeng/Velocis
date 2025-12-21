using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Game.Scripts.Main.Editor.Protobuf
{
    public static class ProtobufGenerator
    {
        [MenuItem("Velocis/Generate Protobuf")]
        private static void Generate()
        {
            var generator = new ProtobufGeneratorInstance();
            generator.Run();
            Debug.Log("Protobuf code generation complete.");
        }
    }
}