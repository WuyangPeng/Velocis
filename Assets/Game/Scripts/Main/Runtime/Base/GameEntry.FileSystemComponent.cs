using Game.Scripts.Main.Runtime.FileSystem;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Base
{
    public partial class GameEntry : MonoBehaviour
    {
        public static FileSystemComponent FileSystemComponent { get; private set; }

        private static void InitFileSystemComponent()
        {
            FileSystemComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<FileSystemComponent>();
        }
    }
}