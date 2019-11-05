using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.PerformanceTesting;

namespace Tests
{
    public class PerformanceTestScript
    {
        /*
        [UnityTest, Performance]
        public IEnumerator RenderPerformance()
        {
            yield return SceneManager.LoadSceneAsync("PlayerMain", LoadSceneMode.Additive);

            SetActiveScene("PlayerMain");

            // Instantiate performance test object in scene
            var renderPerformanceTest =
                SetupPerfTest<DynamicRenderPerformanceMonoBehaviourTest>();

            // allow time to settle before taking measurements
            yield return new WaitForSecondsRealtime(SettleTime);

            // use ProfilerMarkers API from Performance Test Extension
            using (Measure.ProfilerMarkers(SamplerNames))
            {

                // Set CaptureMetrics flag to TRUE; 
                // let's start capturing metrics
                renderPerformanceTest.component.CaptureMetrics = true;

                // Run the MonoBehaviour Test
                yield return renderPerformanceTest;
            }

            yield return SceneManager.UnloadSceneAsync(spiralSceneName);
        }*/
    }
}
