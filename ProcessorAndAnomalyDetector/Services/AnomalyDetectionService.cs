using System.Text.Json;
using ProcessorAndAnomalyDetector.Database;
using ProcessorAndAnomalyDetector.Models;

namespace ProcessorAndAnomalyDetector.Services;

public class AnomalyDetectionService : IAnomalyDetectionService
{
    private readonly IEventsDatabase _eventsDatabase;
    
    public AnomalyDetectionService(IEventsDatabase eventsDatabase)
    {
        _eventsDatabase = eventsDatabase;
    }

    public async Task HandleServerStatisticsMessage(string message)
    {
        try
        {
            var serverStatistics = JsonSerializer.Deserialize<ServerStatistics>(message);
            var recentServerStatistics = await _eventsDatabase.GetRecentEvent(serverStatistics.ServerIdentifier);
            await _eventsDatabase.StoreEvent(serverStatistics);
            Console.WriteLine($"We are comparing the new which is {serverStatistics}");
            Console.WriteLine($"With the most recent which is {recentServerStatistics}");
            
            if (IsMemoryHighUsage(serverStatistics.MemoryUsage, serverStatistics.AvailableMemory))
            {
                SendMessage("HighMemoryUsage");
            }
            else if (recentServerStatistics is not null && 
                     IsMemoryUsageAnomaly(serverStatistics.MemoryUsage,recentServerStatistics.MemoryUsage))
            {
                SendMessage("MemoryUsageAnomaly");
            }
            
            if (IsCpuHighUsage(serverStatistics.CpuUsage))
            {
                SendMessage("HighCpuUsage");
            }
            else if (recentServerStatistics is not null &&
                     IsCpuUsageAnomaly(serverStatistics.CpuUsage,recentServerStatistics.CpuUsage))
            {
                SendMessage("CpuUsageAnomaly");
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private bool IsMemoryHighUsage(double currentMemoryUsage, double currentAvailableMemory)
    {
        try
        {
            const string environmentVariableName = "ANOMALY_DETECTION_CONFIG_MEMORY_USAGE_THRESHOLD_PERCENTAGE";
            var memoryUsageThresholdPercentage = double.Parse(Environment.GetEnvironmentVariable(environmentVariableName));
            return currentMemoryUsage / (currentMemoryUsage + currentAvailableMemory) > memoryUsageThresholdPercentage;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException();
        }
    }
    private bool IsMemoryUsageAnomaly(double currentMemoryUsage, double previousMemoryUsage)
    {
        try
        {
            const string environmentVariableName = "ANOMALY_DETECTION_CONFIG_MEMORY_USAGE_ANOMALY_THRESHOLD_PERCENTAGE";
            var memoryUsageThresholdPercentage = double.Parse(Environment.GetEnvironmentVariable(environmentVariableName));
            return currentMemoryUsage > previousMemoryUsage * (1 + memoryUsageThresholdPercentage);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException();
        }
    }

    private bool IsCpuHighUsage(double currentCpuUsage)
    {
        try
        {
            const string environmentVariableName = "ANOMALY_DETECTION_CONFIG_CPU_USAGE_THRESHOLD_PERCENTAGE";
            var cpuUsageThresholdPercentage = double.Parse(Environment.GetEnvironmentVariable(environmentVariableName));
            return (currentCpuUsage > cpuUsageThresholdPercentage * 100);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException();
        }
    }
    
    private bool IsCpuUsageAnomaly(double currentCpuUsage, double previousCpuUsage)
    {
        try
        {
            const string environmentVariableName = "ANOMALY_DETECTION_CONFIG_CPU_USAGE_ANOMALY_THRESHOLD_PERCENTAGE";
            var cpuUsageAnomalyThresholdPercentage = double.Parse(Environment.GetEnvironmentVariable(environmentVariableName));
            return currentCpuUsage > previousCpuUsage * (1 + cpuUsageAnomalyThresholdPercentage);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException();
        }
    }

    private void SendMessage(string message)
    {
        Console.WriteLine(message);
    }
}