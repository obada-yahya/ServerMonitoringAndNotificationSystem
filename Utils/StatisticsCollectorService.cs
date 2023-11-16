using StatisticsCollector.Models;

namespace StatisticsCollector.Utils;

public class StatisticsCollectorService
{
    private readonly IStatisticsCollectorStrategy _statisticsCollectorStrategy;

    public StatisticsCollectorService(IStatisticsCollectorStrategy statisticsCollectorStrategy)
    {
        _statisticsCollectorStrategy = statisticsCollectorStrategy;
    }

    public ServerStatistics Collect(string? serverIdentifier)
    {
        return new ServerStatistics
        {
            ServerIdentifier = serverIdentifier,
            MemoryUsage = _statisticsCollectorStrategy.CalculateMemoryUsage(),
            AvailableMemory = _statisticsCollectorStrategy.CalculateAvailableMemory(),
            CpuUsage = _statisticsCollectorStrategy.CalculateCpuUsages(),
            Timestamp = DateTime.Now
        };
    }
}