using ReportMicroservice.Api.Models;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using ReportMicroservice.Api.Services.Contracts;

namespace ReportMicroservice.Api.Services
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly RabbitMqClientServices _rabbitMqClientServices;

        public RabbitMqPublisher(RabbitMqClientServices rabbitMqClientServices)
        {
            _rabbitMqClientServices = rabbitMqClientServices;
        }

        public void Publish(RabbitMqPublishModel model)
        {
            var channel = _rabbitMqClientServices.Connect();
            var bodyString = JsonSerializer.Serialize(model);
            var bodyByte = Encoding.UTF8.GetBytes(bodyString);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: RabbitMqClientServices.ExchangeName, routingKey: RabbitMqClientServices.RoutingKey, basicProperties: properties, body: bodyByte);
        }

    }
}
