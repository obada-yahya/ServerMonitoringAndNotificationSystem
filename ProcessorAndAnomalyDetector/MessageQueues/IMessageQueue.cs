namespace ProcessorAndAnomalyDetector.MessageQueues;

public interface IMessageQueue
{
    public Task StartReceivingServerStatisticsAsync();
}