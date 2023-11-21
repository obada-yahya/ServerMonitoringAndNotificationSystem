namespace StatisticsCollector.Models;

public class UnixStatisticsCollectorStrategy : IStatisticsCollectorStrategy
{
    public float CalculateMemoryUsage()
    {
        var memoryInfo = File.ReadLines("/proc/meminfo");

        var totalMemoryInfo = memoryInfo.First();

        var availableMemoryInfo = memoryInfo.Skip(2).First();

        var totalMemoryInKb = float.Parse(totalMemoryInfo.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

        var availableMemoryInKb =
            float.Parse(availableMemoryInfo.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

        return (totalMemoryInKb - availableMemoryInKb) / 1024;
    }

    public float CalculateAvailableMemory()
    {
        var availableMemoryInfo = File.ReadLines("/proc/meminfo")
            .Skip(2)
            .First();

        var availableMemoryInKb =
            float.Parse(availableMemoryInfo.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

        return availableMemoryInKb / 1024;
    }

    public float CalculateCpuUsages(int numberOfTimes = 5, int sleepTime = 200)
    {
        var cpuInfo = File.ReadLines("/proc/stat")
            .First()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ToArray();

        var idleTime = float.Parse(cpuInfo[3]);

        var totalTime = cpuInfo.Aggregate(0.0F,
            (total, num) => total + float.Parse(num));

        return 1 - idleTime / totalTime;
    }
}