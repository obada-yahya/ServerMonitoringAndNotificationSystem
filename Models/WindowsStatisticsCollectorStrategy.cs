using System.Diagnostics;

namespace StatisticsCollector.Models;

public class WindowsStatisticsCollectorStrategy : IStatisticsCollectorStrategy
{
    public float CalculateMemoryUsage()
    {
        return Process.GetProcesses().Sum(process => process.WorkingSet64) / 1048576;
    }

    public float CalculateAvailableMemory()
    {
        using var memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
        var availableMemory = memoryCounter.NextValue();
        return availableMemory;
    }

    public float CalculateCpuUsages(int numberOfTimes = 5,int sleepTime = 200)
    {
        const int timeBeforeStarting = 300;
        var total = 0F;
        using var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        cpuCounter.NextValue();
        cpuCounter.NextValue();
        Thread.Sleep(timeBeforeStarting);
        for (var i = 0; i < numberOfTimes; i++)
        {
            var cpuUsage = cpuCounter.NextValue();
            total += cpuUsage;
            Thread.Sleep(sleepTime);
        }
        return total / numberOfTimes;
    }
}
