using RabbitMQ.Client;

namespace ReportMicroservice.Api.Services
{
    public class RabbitMqClientServices : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public static string ExchangeName = "ContactsExcelDirectExchange";
        public static string RoutingKey = "contacts-excel-route-file";
        public static string QueueName = "queue-contacts-excel-file";

        private readonly ILogger<RabbitMqClientServices> _logger;

        public RabbitMqClientServices(ConnectionFactory connectionFactory, ILogger<RabbitMqClientServices> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();
            if (_channel != null && _channel.IsOpen)
            {
                return _channel;
            }

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, "direct", true, false);
            _channel.QueueDeclare(QueueName, true, false, false, null);
            _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingKey);

            _logger.LogInformation("Rabbitmq ile bağlantı kuruldu");

            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("Rabbitmq ile bağlantı koptu");
        }
    }
}
