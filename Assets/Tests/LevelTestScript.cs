using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.PerformanceTesting;

namespace Tests
{
    public class LevelTestScript
    {
        [Test]
        public void TestScoreAdditionEasy()
        {
            // Assign
            var score = new Score(); // Default score = 0

            // Act
            score.AddScoreAccordingToDifficulty("Easy");

            // Assert
            Assert.AreEqual(10, score.GetScore());

            Debug.Log("Test 1 ====" + score.GetScore());
        }

        [Test]
        public void TestScoreAdditionNormal()
        {
            // Assign
            var score = new Score(); // Default score = 0

            // Act
            score.AddScoreAccordingToDifficulty("Normal");

            // Assert
            Assert.AreEqual(20, score.GetScore());

            Debug.Log("Test 2 ====" + score.GetScore());
        }

        [Test]
        public void TestScoreAdditionHard()
        {
            // Assign
            var score = new Score(); // Default score = 0

            // Act
            score.AddScoreAccordingToDifficulty("Hard");

            // Assert
            Assert.AreEqual(40, score.GetScore());

            Debug.Log("Test 3 ====" + score.GetScore());
        }

        [Test]
        public void TestResetScore()
        {
            // Assign
            var score = new Score(); // Default score = 0

            // Act
            score.ResetScore();

            // Assert
            Assert.AreEqual(0, score.GetScore());

            Debug.Log("Test 4 ====" + score.GetScore());
        }
    }
}
