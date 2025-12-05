using UnityEngine;

namespace Game.Scripts.Main.Runtime.Procedure.Scene
{
    public enum SceneType
    {
        [InspectorName("菜单")] Menu = 1,
        [InspectorName("主场景（测试）")] Main = 2,
        [InspectorName("创建角色")] Create = 3,
        [InspectorName("初始化游戏")] InitGame = 4,
        [InspectorName("主场景")] Home = 5,
        [InspectorName("战斗")] Battle = 6
    }
}