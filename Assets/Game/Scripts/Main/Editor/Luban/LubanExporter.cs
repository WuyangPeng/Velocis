using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Game.Scripts.Main.Editor.Luban
{
    public static class LubanExporter
    {
        public static void Export(string target)
        {
            var parent = Directory.GetParent(Application.dataPath);
            if (parent == null)
            {
                Debug.Log("Can't find Luban.exe");
                return;
            }

            var projectRoot = parent.FullName;
            var toolsDir = Path.Combine(projectRoot, "Tools", "Luban");
            var lubanDll = Path.Combine(toolsDir, "Luban.dll");
            var lubanExe = Path.Combine(toolsDir, "Luban.exe");

            var cmd = "";
            var args = "";

            if (File.Exists(lubanDll))
            {
                cmd = "dotnet";
                args = $"\"{lubanDll}\"";
            }
            else if (File.Exists(lubanExe))
            {
                cmd = lubanExe;
                args = "";
            }
            else
            {
                Debug.Log($"Luban not found at {toolsDir}. Expected Luban.dll or Luban.exe");
                return;
            }

            var confPath = Path.Combine(Application.dataPath, "Game", "luban", "luban.conf");
            var codeOutputDir = Path.Combine(Application.dataPath, "Game", "Scripts", "Main", "Runtime", "Luban");
            var dataOutputDir = Path.Combine(Application.dataPath, "Game", "Bin");

            if (!Directory.Exists(codeOutputDir))
            {
                Directory.CreateDirectory(codeOutputDir);
            }

            if (!Directory.Exists(dataOutputDir))
            {
                Directory.CreateDirectory(dataOutputDir);
            }

            args += " -c cs-bin";
            args += " -d bin";

            // 构建参数
            args += $" -t {target}";
            args += $" --conf \"{confPath}\"";

            // 将输出目录作为自定义参数 (xargs) 传递，这在模板中通常会用到
            args += $" -x outputCodeDir=\"{codeOutputDir}\"";
            args += $" -x outputDataDir=\"{dataOutputDir}\"";

            RunCommand(cmd, args);
        }

        private static void RunCommand(string cmd, string args)
        {
            Debug.Log($"Running Luban: {cmd} {args}");

            var startInfo = new ProcessStartInfo
            {
                FileName = cmd,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Directory.GetParent(Application.dataPath)!.FullName
            };

            using var process = Process.Start(startInfo);
            if (process == null)
            {
                Debug.Log("Can't find Luban.exe");
                return;
            }

            var output = "";
            var error = "";

            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    output += e.Data + "\n";
                }
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    error += e.Data + "\n";
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
                Debug.Log($"Luban Export Failed (ExitCode {process.ExitCode}):\n{output}\n{error}");
            }
        }
    }
}