using System.Collections.Generic;
using System.IO;
using UnityGameFramework.Runtime;
using GameFramework.FileSystem;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using Path = System.IO.Path;

namespace Game.Scripts.Main.Runtime.FileSystem
{

    public class FileSystemComponent : GameFrameworkComponent
    {
        private readonly Dictionary<string, IFileSystem> fileSystem = new();

        public IFileSystem GetFileSystem(string rootPath)
        {
            return File.Exists(rootPath) ? GameEntry.FileSystem.LoadFileSystem(rootPath, FileSystemAccess.ReadWrite) : CreateFileSystem(rootPath);
        }

        public IFileSystem CreateFileSystem(string rootPath)
        {
            var result = GameEntry.FileSystem.CreateFileSystem(rootPath, FileSystemAccess.ReadWrite, 1024, 1024);
            result.WriteFile("$dummy", new byte[] { 1 });

            return result;
        }

        public IFileSystem CreateFileSystem(string directory, string pathName)
        {
            Directory.CreateDirectory(directory);

            var rootPath = Path.Combine(directory, pathName);

            if (fileSystem.TryGetValue(rootPath, out var result))
            {
                return result;
            }

            var file = GetFileSystem(rootPath);

            if (file != null)
            {
                fileSystem.Add(rootPath, file);
            }

            return file;
        }
    }
}