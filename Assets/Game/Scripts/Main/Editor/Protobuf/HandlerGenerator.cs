using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Main.Editor.Protobuf
{
    public static class HandlerGenerator
    {
        [MenuItem("Velocis/Generate Protobuf Handlers")]
        public static void Generate()
        {
            var generator = new HandlerGeneratorInstance();
            generator.Run();
            Debug.Log("Protobuf Handler generation complete.");
        }
    }
}