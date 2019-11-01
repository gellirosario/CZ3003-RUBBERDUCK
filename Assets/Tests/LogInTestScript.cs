using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class LogInTestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void LogInTestScriptSimplePasses()
        {
            // Use the Assert class to test conditions

            // Arrange:
            var analyzer = new LogInController();
            // Act:
            var result = analyzer.LogIn();
            // Assert:
            Assert.AreEqual(true, result); // Why 123?
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator LogInTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
