using SignalREventConsumerService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapHub<AlertHub>("Alert");

app.Run();