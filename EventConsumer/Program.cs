using EventConsumer.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var signalRHubUrl = Environment.GetEnvironmentVariable("SIGNALR_CONFIG__SIGNALR_URL");
        var service = new SignalRClient(signalRHubUrl);
        service.ReceiveAlertMessages();
        Console.ReadLine();
    }
}