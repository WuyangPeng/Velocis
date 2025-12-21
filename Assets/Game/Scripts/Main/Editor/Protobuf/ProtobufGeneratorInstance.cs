using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Game.Scripts.Main.Editor.Protobuf
{
    public class ProtobufGeneratorInstance
    {
        private readonly DirectoryInfo _directory = Directory.GetParent(Application.dataPath);

        public void Run()
        {
            if (_directory == null)
            {
                Debug.LogError("directory not found at: " + Application.dataPath);
                return;
            }

            var projectRoot = _directory.FullName;
            var protocPath = Path.Combine(projectRoot, "Assets", "Plugins", "libs", "Google.Protobuf", "protoc.exe");
            var protoDirectory = Path.Combine(projectRoot, "Assets", "Game");
            var outputDirectory = Path.Combine(projectRoot, "Assets", "Game", "Scripts", "Main", "Runtime", "Protobuf");

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
                var fileDir = Path.GetDirectoryName(protoFile);
                var relativeDir = Path.GetRelativePath(protoDirectory, fileDir);

                var fileSpecificOutputDirectory = outputDirectory;
                if (!string.IsNullOrEmpty(relativeDir) && relativeDir != ".")
                {
                    fileSpecificOutputDirectory = Path.Combine(outputDirectory, relativeDir);
                }

                Directory.CreateDirectory(fileSpecificOutputDirectory);

                var startInfo = new ProcessStartInfo
                {
                    FileName = protocPath,
                    Arguments = $"--csharp_out=\"{fileSpecificOutputDirectory}\" --proto_path=\"{protoDirectory}\" \"{protoFile}\"",
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo);
                if (process == null)
                {
                    Debug.LogError("process is null.");
                    return;
                }

                process.WaitForExit();
                var error = process.StandardError.ReadToEnd();
                if (process.ExitCode != 0)
                {
                    Debug.LogError($"Error generating code from {protoFile}:\n{error}");
                }
                else
                {
                    Debug.Log($"Successfully generated code for: {protoFile}");
                }

                AssetDatabase.Refresh();
            }
        }
    }
}