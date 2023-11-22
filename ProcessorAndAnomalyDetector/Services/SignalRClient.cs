using Microsoft.AspNetCore.SignalR.Client;

namespace ProcessorAndAnomalyDetector.Services;

public class SignalRClient : IRealTimeAlertSender
{
    private readonly HubConnection _hubConnection;

    public SignalRClient(string hubUrl)
    {
        _hubConnection = EstablishHubConnection(hubUrl);
        _hubConnection.StartAsync();
    }

    private HubConnection EstablishHubConnection(string hubUrl)
    {
        return new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();
    }

    public async Task SendAlertMessage(string message)
    {
        await _hubConnection.InvokeAsync("SendAlertMessage", message);
    }
}