namespace EventConsumer.Services;

public interface IReceiveRealTimeCommunication
{
    public Task ReceiveAlertMessage();
}