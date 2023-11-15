namespace Something.Models;

public interface IStatisticsCollectorStrategy
{
    public float CalculateMemoryUsage();
    public float CalculateAvailableMemory();
    public float CalculateCpuUsages(int numberOfTimes = 5, int sleepTime = 200);
}