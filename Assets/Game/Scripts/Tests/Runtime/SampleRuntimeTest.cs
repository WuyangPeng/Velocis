using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Game.Scripts.Tests.Runtime
{
    public class SampleRuntimeTest
    {
        [UnityTest]
        public IEnumerator SampleRuntimeTestWithEnumeratorPasses()
        {
            // 这是一个 PlayMode 测试示例
            // 它会在游戏运行时执行
            yield return null;
            Assert.IsTrue(true);
        }
    }
}
