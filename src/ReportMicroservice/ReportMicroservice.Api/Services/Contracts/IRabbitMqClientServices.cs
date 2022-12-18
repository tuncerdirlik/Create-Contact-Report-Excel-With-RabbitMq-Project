using RabbitMQ.Client;

namespace ReportMicroservice.Api.Services.Contracts
{
    public interface IRabbitMqClientServices
    {
        IModel Connect();
    }
}
