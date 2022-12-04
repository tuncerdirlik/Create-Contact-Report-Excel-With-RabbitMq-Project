using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateExcelWorkerService.Services
{
    public class RabbitMqClientServices : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

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
