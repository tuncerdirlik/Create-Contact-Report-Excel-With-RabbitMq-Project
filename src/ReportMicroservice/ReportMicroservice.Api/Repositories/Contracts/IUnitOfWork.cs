namespace ReportMicroservice.Api.Repositories.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IReportFileRepository ReportFileRepository { get; }

        Task SaveAsync();
    }
}
