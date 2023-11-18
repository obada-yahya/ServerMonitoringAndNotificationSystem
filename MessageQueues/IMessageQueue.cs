namespace StatisticsCollector.MessageQueues;

public interface IMessageQueue
{
    public Task StartSendingServerStatisticsPeriodicallyAsync(int intervalSeconds);
}