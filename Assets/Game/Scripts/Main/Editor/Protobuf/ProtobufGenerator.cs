using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Game.Scripts.Main.Editor.Protobuf
{
    public sealed class ProtobufGenerator
    {
        [MenuItem("Velocis/Generate Protobuf")]
        private static void Generate()
        {
            var projectRoot = Directory.GetParent(Application.dataPath).FullName;
            var protocPath = Path.Combine(projectRoot, "Assets", "Plugins", "libs", "Google.Protobuf", "protoc.exe");
            var protoDirectory = Path.Combine(projectRoot, "Assets", "Game", "proto");
            var outputDirectory = Path.Combine(projectRoot, "Assets", "Game", "Scripts","Main","Runtime","Protobuf");

            if (!File.Exists(protocPath))
            {
                Debug.LogError("protoc.exe not found at: " + protocPath);
                return;
            }

            if (!Directory.Exists(protoDirectory))
            {
                Debug.LogError("Proto directory not found at: " + protoDirectory);
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var protoFiles = Directory.GetFiles(protoDirectory, "*.proto", SearchOption.AllDirectories);

            foreach (var protoFile in protoFiles)
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = protocPath,
                    Arguments = $"--csharp_out=\"{outputDirectory}\" --proto_path=\"{protoDirectory}\" \"{protoFile}\"",
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    var error = process.StandardError.ReadToEnd();
                    if (process.ExitCode != 0)
                    {
                        Debug.LogError($"Error generating code from {protoFile}:\n{error}");
                    }
                }
            }

            Debug.Log("Protobuf code generation complete.");
            AssetDatabase.Refresh();
        }
    }
}