using System;
using System.Threading;
using NLoad;

namespace HRTools.Presentation.Tests
{
    public class BasePerformanceTest
    {
        protected LoadTestResult Test<T>() where T : ITest, new()
        {
            LoadTest<T> test =
                new LoadTest<T>(new LoadTestConfiguration
                {
                    NumberOfThreads = 10,
                    //Duration = TimeSpan.FromMinutes(3),
                    Duration = TimeSpan.FromSeconds(10),
                    StartImmediately = true,
                    DelayBetweenThreadStart = TimeSpan.Zero// TimeSpan.FromMilliseconds(100)
                }, CancellationToken.None);

            //test.Heartbeat += (s, e) => Console.WriteLine(e.Throughput);

            return test.Run();
        }
    }
}