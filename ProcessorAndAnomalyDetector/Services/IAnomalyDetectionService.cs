namespace ProcessorAndAnomalyDetector.Services;

public interface IAnomalyDetectionService
{
    public Task HandleServerStatisticsMessage(string message);
}