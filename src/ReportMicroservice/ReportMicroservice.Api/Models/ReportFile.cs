namespace ReportMicroservice.Api.Models
{
    public class ReportFile
    {
        public int Id { get; set; }
        public string? FileName { get; set; } 
        public string? FilePath { get; set; } 
        public bool IsCompleted { get; set; } 
    }
}
