using Microsoft.AspNetCore.SignalR;

namespace SignalREventConsumerService;

public class AlertHub : Hub
{
    private readonly ILogger<AlertHub> _logger;

    public AlertHub(ILogger<AlertHub> logger)
    {
        _logger = logger;
    }

    public async Task SendAlertMessage(string message)
    {
        try
        {
            const string receiveMethodName = "ReceiveAlertMessage";
            await Clients.All.SendAsync(receiveMethodName, message);
        }
        catch (Exception e)
        {
            _logger.LogCritical("Failed to Send Alert Message");
        }
    }
}