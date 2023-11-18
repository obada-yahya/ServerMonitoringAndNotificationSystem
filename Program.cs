using StatisticsCollector.MessageQueues;

public class Program
{
    public async static Task Main(string[] args)
    {
        await Task.Delay(25000);
        var serverIdentifier = Environment.GetEnvironmentVariable("SERVERSTATISTICSCONFIG_SERVERIDENTIFIER");
        var samplingIntervalSeconds = int.Parse(Environment.GetEnvironmentVariable("SERVERSTATISTICSCONFIG_SAMPLINGINTERVALSECONDS"));
        Console.WriteLine(serverIdentifier);
        Console.WriteLine(samplingIntervalSeconds);
        Console.WriteLine(Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING"));
        var exchangeName = "ServerStatistics";
        var queueName = "ServerStatisticsQueue";
        var messageQueue = new RabbitMq(exchangeName, queueName, serverIdentifier);
        messageQueue.StartSendingServerStatisticsPeriodicallyAsync(samplingIntervalSeconds).Wait();
    }
}