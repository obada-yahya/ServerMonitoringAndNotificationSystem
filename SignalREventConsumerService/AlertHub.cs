using Microsoft.AspNetCore.SignalR;

namespace SignalREventConsumerService;

public class AlertHub : Hub
{
    public async Task SendAlertMessage(string message)
    {
        const string receiveMethodName = "ReceiveAlertMessage";
        await Clients.All.SendAsync(receiveMethodName, message);
    }
}