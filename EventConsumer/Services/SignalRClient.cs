using Microsoft.AspNetCore.SignalR.Client;

namespace EventConsumer.Services;

public class SignalRClient : IReceiveRealTimeCommunication
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
                .WithAutomaticReconnect()
                .Build();
    }

    public Task ReceiveAlertMessages()
    {
        _hubConnection.On<string>("ReceiveAlertMessage", eventData =>
        {
            Console.WriteLine($"ReceiveAlertMessage : {eventData}");
        });
        return Task.CompletedTask;
    }
}