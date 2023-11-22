using StatisticsCollector.MessageQueues;

public class Program
{
    public static void Main(string[] args)
    {
        var serverIdentifier = Environment.GetEnvironmentVariable("SERVERSTATISTICSCONFIG_SERVERIDENTIFIER");
        var samplingIntervalSeconds = int.Parse(Environment.GetEnvironmentVariable("SERVERSTATISTICSCONFIG_SAMPLINGINTERVALSECONDS"));
        var exchangeName = "ServerStatistics";
        var queueName = "ServerStatisticsQueue";
        var messageQueue = new RabbitMq(exchangeName, queueName, serverIdentifier);
        messageQueue.StartSendingServerStatisticsPeriodicallyAsync(samplingIntervalSeconds).Wait();
    }
}