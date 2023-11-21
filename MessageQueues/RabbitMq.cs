using System.Text;
using RabbitMQ.Client;
using StatisticsCollector.Utils;
using Exception = System.Exception;

namespace StatisticsCollector.MessageQueues;

public class RabbitMq : IMessageQueue
{
    private IConnection? _connection;
    private readonly string _exchangeName;
    private readonly string _queueName;
    private readonly string _serverIdentifier;
    
    public RabbitMq(string exchangeName, string queueName, string serverIdentifier)
    {
        _exchangeName = exchangeName;
        _queueName = queueName;
        _serverIdentifier = serverIdentifier;
    }

    private IConnection? CreateConnection()
    {
        var rabbitMqStringConnection = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING");
        var clientProvidedName = "Rabbit Statistics Sender";
        var maxConnectionAttempts  = 30;
        var timeDelayBetweenAttempts = 1000;
        while (maxConnectionAttempts  >= 0)
        {
            try
            {
                return new ConnectionFactory()
                {
                    Uri = new Uri(rabbitMqStringConnection),
                    ClientProvidedName = clientProvidedName
                }.CreateConnection();
            }
            catch (Exception)
            {
                maxConnectionAttempts --;
                Thread.Sleep(timeDelayBetweenAttempts);
            }
        }
        Console.WriteLine("Connection to RabbitMq Server has failed.");
        return null;
    }
    
    public Task StartSendingServerStatisticsPeriodicallyAsync(int intervalSeconds)
    {
        return Task.Run(() => SendServerStatisticsPeriodically(intervalSeconds));
    }
    
    private void SendServerStatisticsPeriodically(int intervalSeconds)
    {
        try
        {
            var statisticsCollectorService = new StatisticsCollectorService(StatisticsCollectorFactory.CreateCollector());
            var routingKey = $"{_exchangeName}.{_serverIdentifier}";
            
            _connection = CreateConnection();
            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare(_exchangeName,ExchangeType.Direct);
            channel.QueueDeclare(_queueName, false, false, false, null);
            channel.QueueBind(_queueName, _exchangeName, routingKey, null);
            var i = 0;
            while(true)
            {
                Console.WriteLine($"Sending message #{i++}");
                var serverStatistics = statisticsCollectorService.Collect(_serverIdentifier);
                var messageBodyBytes = Encoding.UTF8.GetBytes(serverStatistics.ToString());
                Console.WriteLine(serverStatistics);
                channel.BasicPublish(_exchangeName,routingKey,null,messageBodyBytes);
                Thread.Sleep(intervalSeconds * 1000);
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}