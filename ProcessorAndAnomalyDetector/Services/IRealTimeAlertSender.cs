namespace ProcessorAndAnomalyDetector.Services;

public interface IRealTimeAlertSender
{
    public Task SendAlertMessage(string message);
}