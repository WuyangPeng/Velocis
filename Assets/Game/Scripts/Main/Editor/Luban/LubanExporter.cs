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

            var outputDirectories = PrepareDirectories();
            var confPath = Path.Combine(Application.dataPath, "Game", "luban", "luban.conf");

            BuildArguments(lubanInfo, target, confPath, outputDirectories);

            RunCommand(lubanInfo);
        }

        private static LubanOutputDirectories PrepareDirectories()
        {
            var codeOutputDirectory = Path.Combine(Application.dataPath, "Game", "Scripts", "Hotfix", "HotfixCommon", "Luban");
            var dataOutputDirectory = Path.Combine(Application.dataPath, "Game", "Bin");

            if (!Directory.Exists(codeOutputDirectory))
            {
                Directory.CreateDirectory(codeOutputDirectory);
            }

            if (!Directory.Exists(dataOutputDirectory))
            {
                Directory.CreateDirectory(dataOutputDirectory);
            }

            return new LubanOutputDirectories(codeOutputDirectory, dataOutputDirectory);
        }

        private static void BuildArguments(LubanInfo lubanInfo, string target, string confPath, LubanOutputDirectories outputDirectory)
        {
            lubanInfo.AddArgument(" -c cs-bin");
            lubanInfo.AddArgument(" -d bin");

            // 构建参数
            lubanInfo.AddArgument($" -t {target}");
            lubanInfo.AddArgument($" --conf \"{confPath}\"");

            // 将输出目录作为自定义参数传递，这在模板中通常会用到
            lubanInfo.AddArgument($" -x outputCodeDir=\"{outputDirectory.CodeOutputDirectory}\"");
            lubanInfo.AddArgument($" -x outputDataDir=\"{outputDirectory.DataOutputDirectory}\"");
        }

        private static void RunCommand(LubanInfo lubanInfo)
        {
            var startInfo = CreateProcessStartInfo(lubanInfo);
            var result = ExecuteProcess(startInfo);
            
            HandleProcessResult(result);
        }

        private static ProcessStartInfo CreateProcessStartInfo(LubanInfo lubanInfo)
        {
            Debug.Log($"Running Luban: {lubanInfo.GetCommand()} {lubanInfo.GetArgument()}");

            return new ProcessStartInfo
            {
                FileName = lubanInfo.GetCommand(),
                Arguments = lubanInfo.GetArgument(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = LubanDirectory.GetProjectRoot()
            };
        }

        private static ProcessResult ExecuteProcess(ProcessStartInfo startInfo)
        {
            using var process = Process.Start(startInfo);
            if (process == null)
            {
                Debug.LogError("Can't find Luban.exe");
                return new ProcessResult(-1, "", "Process.Start returned null");
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

            return new ProcessResult(process.ExitCode, output, error);
        }

        private static void HandleProcessResult(ProcessResult result)
        {
            if (result.ExitCode == 0)
            {
                Debug.Log($"Luban Export Success:\n{result.Output}");
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError($"Luban Export Failed (ExitCode {result.ExitCode}):\n{result.Output}\n{result.Error}");
            }
        }
    }
}