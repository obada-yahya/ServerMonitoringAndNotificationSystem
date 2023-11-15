using StatisticsCollector.Models;

namespace StatisticsCollector.Utils;

public class StatisticsCollectorService
{
    private readonly IStatisticsCollectorStrategy _statisticsCollectorStrategy;

    public StatisticsCollectorService(IStatisticsCollectorStrategy statisticsCollectorStrategy)
    {
        _statisticsCollectorStrategy = statisticsCollectorStrategy;
    }

    public ServerStatistics Collect()
    {
        return new ServerStatistics
        {
            MemoryUsage = _statisticsCollectorStrategy.CalculateMemoryUsage(),
            AvailableMemory = _statisticsCollectorStrategy.CalculateAvailableMemory(),
            CpuUsage = _statisticsCollectorStrategy.CalculateCpuUsages(),
            Timestamp = DateTime.Now
        };
    }
}