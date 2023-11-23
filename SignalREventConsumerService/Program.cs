using SignalREventConsumerService;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapHub<AlertHub>("Alert");

app.Run();