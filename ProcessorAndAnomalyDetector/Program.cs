using ProcessorAndAnomalyDetector.Database;
using ProcessorAndAnomalyDetector.MessageQueues;
using ProcessorAndAnomalyDetector.Services;

namespace ProcessorAndAnomalyDetector;

public class Program
{
    public static void Main(string[] args)
    {
        var mongoDbConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONFIG__CONNECTION_STRING");
        var anomalyDetectionService = new AnomalyDetectionService(new EventsDatabase(mongoDbConnectionString));
        var msgQueue = new RabbitMq(anomalyDetectionService,"ServerStatistics","ServerStatisticsQueue");
        msgQueue.StartReceivingServerStatisticsAsync().Wait();
    }
}