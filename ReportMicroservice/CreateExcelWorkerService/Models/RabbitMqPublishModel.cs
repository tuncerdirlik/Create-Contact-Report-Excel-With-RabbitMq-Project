namespace CreateExcelWorkerService.Models
{
    public class RabbitMqPublishModel
    {
        public ReportFile ReportFile { get; set; }
        public List<ContactDto> Contacts { get; set; }
    }
}
