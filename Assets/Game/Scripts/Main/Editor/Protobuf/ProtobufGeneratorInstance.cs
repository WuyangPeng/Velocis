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
        private readonly string _outputDirectory;
        private readonly string _protocPath;
        private readonly string _protoDirectory;

        public ProtobufGeneratorInstance()
        {
            if (_directory == null)
            {
                Debug.LogError("directory not found at: " + Application.dataPath);
                return;
            }

            var projectRoot = _directory.FullName;
            _protocPath = Path.Combine(projectRoot, "Assets", "Plugins", "libs", "Google.Protobuf", "protoc.exe");
            _protoDirectory = Path.Combine(projectRoot, "Assets", "Game");
            _outputDirectory = Path.Combine(projectRoot, "Assets", "Game", "Scripts", "Main", "Runtime", "Protobuf");
        }

        public void Run()
        {
            if (!ValidatePaths(_protocPath, _protoDirectory))
            {
                return;
            }

            EnsureDirectoryExists(_outputDirectory);

            var protoFiles = Directory.GetFiles(_protoDirectory, "*.proto", SearchOption.AllDirectories);

            foreach (var protoFile in protoFiles)
            {
                GenerateCodeForFile(protoFile, _protocPath, _protoDirectory, _outputDirectory);
            }

            AssetDatabase.Refresh();
        }

        private static bool ValidatePaths(string protocPath, string protoDirectory)
        {
            if (!File.Exists(protocPath))
            {
                Debug.LogError("protoc.exe not found at: " + protocPath);
                return false;
            }

            if (Directory.Exists(protoDirectory))
            {
                return true;
            }

            Debug.LogError("Proto directory not found at: " + protoDirectory);

            return false;
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void GenerateCodeForFile(string protoFile, string protocPath, string protoDirectory, string outputDirectory)
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
        }
    }
}