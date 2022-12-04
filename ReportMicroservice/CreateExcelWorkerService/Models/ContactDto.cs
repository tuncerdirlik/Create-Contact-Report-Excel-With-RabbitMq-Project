namespace CreateExcelWorkerService.Models
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string email { get; set; }
        public string web { get; set; }
    }
}
