using CreateExcelWorkerService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;
using System.Threading.Channels;
using CreateExcelWorkerService.Models;
using System.Data;
using ClosedXML.Excel;
using FastMember;

namespace CreateExcelWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private RabbitMqClientServices _rabbitMqClientServices;
        private readonly IServiceProvider _serviceProvider;
        private IModel _channel;

        public Worker(ILogger<Worker> logger, RabbitMqClientServices rabbitMqClientServices, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _rabbitMqClientServices = rabbitMqClientServices;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMqClientServices.Connect();
            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(RabbitMqClientServices.QueueName, false, consumer);

            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var publishModel = JsonSerializer.Deserialize<RabbitMqPublishModel>(Encoding.UTF8.GetString(@event.Body.ToArray()));

                using var ms = new MemoryStream();

                DataTable dt = new DataTable() { TableName = "contacts" };
                using (var reader = ObjectReader.Create(publishModel.Contacts))
                {
                    dt.Load(reader);
                }

                var wb = new XLWorkbook();
                wb.Worksheets.Add(dt);
                
                wb.SaveAs(ms);

                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                multipartFormDataContent.Add(new ByteArrayContent(ms.ToArray()), "file", string.Concat(publishModel.ReportFile.FileName, ".xlsx"));

                var baseUrl = $"http://localhost:5274/api/Contacts/UploadExcel?reportFileId={publishModel.ReportFile.Id}&reportFileName={publishModel.ReportFile.FileName}";
                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsync(baseUrl, multipartFormDataContent);

                if (response.IsSuccessStatusCode)
                {
                    _channel.BasicAck(@event.DeliveryTag, false);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}