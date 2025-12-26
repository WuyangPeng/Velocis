using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Game.Scripts.Main.Editor.Luban
{
    public static class LubanExporter
    {
        private static readonly LubanDirectory LubanDirectory = new();

        private static LubanInfo GetLubanInfo()
        {
            if (!LubanDirectory.IsEffective())
            {
                return null;
            }

            var lubanDll = LubanDirectory.GetLubanDll();
            var lubanExe = LubanDirectory.GetLubanExe();

            if (File.Exists(lubanDll))
            {
                return new LubanInfo("dotnet", $"\"{lubanDll}\"");
            }

            if (File.Exists(lubanExe))
            {
                return new LubanInfo(lubanExe, "");
            }

            Debug.LogError($"Luban not found at {LubanDirectory.GetToolsDirectory()}. Expected Luban.dll or Luban.exe");

            return null;
        }

        public static void Export(string target)
        {
            var lubanInfo = GetLubanInfo();
            if (lubanInfo == null)
            {
                return;
            }

            var confPath = Path.Combine(Application.dataPath, "Game", "luban", "luban.conf");
            var codeOutputDirectory = Path.Combine(Application.dataPath, "Game", "Scripts", "Main", "Runtime", "Luban");
            var dataOutputDirectory = Path.Combine(Application.dataPath, "Game", "Bin");

            if (!Directory.Exists(codeOutputDirectory))
            {
                Directory.CreateDirectory(codeOutputDirectory);
            }

            if (!Directory.Exists(dataOutputDirectory))
            {
                Directory.CreateDirectory(dataOutputDirectory);
            }

            lubanInfo.AddArgument(" -c cs-bin");
            lubanInfo.AddArgument(" -d bin");

            // 构建参数
            lubanInfo.AddArgument($" -t {target}");
            lubanInfo.AddArgument($" --conf \"{confPath}\"");

            // 将输出目录作为自定义参数 (xargs) 传递，这在模板中通常会用到
            lubanInfo.AddArgument($" -x outputCodeDir=\"{codeOutputDirectory}\"");
            lubanInfo.AddArgument($" -x outputDataDir=\"{dataOutputDirectory}\"");

            RunCommand(lubanInfo);
        }

        private static void RunCommand(LubanInfo lubanInfo)
        {
            Debug.Log($"Running Luban: {lubanInfo.GetCommand()} {lubanInfo.GetArgument()}");

            var startInfo = new ProcessStartInfo
            {
                FileName = lubanInfo.GetCommand(),
                Arguments = lubanInfo.GetArgument(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = LubanDirectory.GetProjectRoot()
            };

            using var process = Process.Start(startInfo);
            if (process == null)
            {
                Debug.LogError("Can't find Luban.exe");
                return;
            }

            var output = "";
            var error = "";

            process.OutputDataReceived += (_, data) =>
            {
                if (data.Data != null)
                {
                    output += data.Data + "\n";
                }
            };

            process.ErrorDataReceived += (_, data) =>
            {
                if (data.Data != null)
                {
                    error += data.Data + "\n";
                }
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                Debug.Log($"Luban Export Success:\n{output}");
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError($"Luban Export Failed (ExitCode {process.ExitCode}):\n{output}\n{error}");
            }
        }
    }
}