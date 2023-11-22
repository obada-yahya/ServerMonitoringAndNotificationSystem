using EventConsumer.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var service = new SignalRClient("http://localhost:5124/alert");
        service.ReceiveAlertMessage();
        Console.ReadLine();
    }
}