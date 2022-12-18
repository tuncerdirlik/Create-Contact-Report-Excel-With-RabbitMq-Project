using ReportMicroservice.Api.Models;

namespace ReportMicroservice.Api.Services.Contracts
{
    public interface IRabbitMqPublisher
    {
        void Publish(RabbitMqPublishModel model);
    }
}
