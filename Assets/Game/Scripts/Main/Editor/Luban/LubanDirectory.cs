using System.IO;
using UnityEngine;

namespace Game.Scripts.Main.Editor.Luban
{
    public class LubanDirectory
    {
        private readonly string _lubanDll;
        private readonly string _lubanExe;
        private readonly DirectoryInfo _parent = Directory.GetParent(Application.dataPath);
        private readonly string _projectRoot;
        private readonly string _toolsDirectory;

        public LubanDirectory()
        {
            if (_parent == null)
            {
                Debug.LogError("Can't find Luban.exe");
                return;
            }

            _projectRoot = _parent.FullName;
            _toolsDirectory = Path.Combine(_projectRoot, "Tools", "Luban");
            _lubanDll = Path.Combine(_toolsDirectory, "Luban.dll");
            _lubanExe = Path.Combine(_toolsDirectory, "Luban.exe");
        }

        public bool IsEffective()
        {
            return _parent != null;
        }

        public string GetLubanDll()
        {
            return _lubanDll;
        }

        public string GetProjectRoot()
        {
            return _projectRoot;
        }

        public string GetLubanExe()
        {
            return _lubanExe;
        }

        public string GetToolsDirectory()
        {
            return _toolsDirectory;
        }
    }
}