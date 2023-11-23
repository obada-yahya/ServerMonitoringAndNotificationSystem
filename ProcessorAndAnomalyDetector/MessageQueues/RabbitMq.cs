using System.Text;
using ProcessorAndAnomalyDetector.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProcessorAndAnomalyDetector.MessageQueues;

public class RabbitMq : IMessageQueue
{
    private IConnection? _connection;
    private readonly IAnomalyDetectionService _anomalyDetectionService;
    private readonly string _exchangeName;
    private readonly string _queueName;

    public RabbitMq(IAnomalyDetectionService anomalyDetectionService, string exchangeName, string queueName)
    {
        _anomalyDetectionService = anomalyDetectionService;
        _exchangeName = exchangeName;
        _queueName = queueName;
    }
    
    private IConnection? CreateConnection()
    {
        var rabbitMqStringConnection = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING");
        const string clientProvidedName = "Rabbit Statistics Receiver";
        const int timeDelayBetweenAttempts = 1000;
        var maxConnectionAttempts = 30;
        while (maxConnectionAttempts >= 0)
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
                maxConnectionAttempts--;
                Thread.Sleep(timeDelayBetweenAttempts);
            }
        }
        Console.WriteLine("Connection to RabbitMq Server has failed.");
        return null;
    }
    
    public Task StartReceivingServerStatisticsAsync()
    {
        return Task.Run(SendServerStatisticsPeriodically);
    }
    
    private void SendServerStatisticsPeriodically()
    {
        try
        {
            _connection = CreateConnection();
            var channel = _connection.CreateModel();
            const string serverIdentifier = "*";
            var routingKey = $"{_exchangeName}.{serverIdentifier}";

            channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(_queueName, false, false, false, null);
            channel.QueueBind(_queueName, _exchangeName, routingKey, null);

            channel.BasicQos(0, 0, false);
            var consumer = new EventingBasicConsumer(channel);
            var cnt = 0;
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message #{cnt++} Received");
                _anomalyDetectionService.HandleServerStatisticsMessage(message);
                channel.BasicAck(args.DeliveryTag, false);
            };
            var consumerTag = channel.BasicConsume(_queueName, false, consumer);
            Console.ReadLine();
            channel.BasicCancel(consumerTag);
            channel.Close();
            _connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}