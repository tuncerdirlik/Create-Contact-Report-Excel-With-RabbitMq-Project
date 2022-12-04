using ReportMicroservice.Api.DbContexts;
using ReportMicroservice.Api.Repositories.Contracts;

namespace ReportMicroservice.Api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IReportFileRepository _reportFileRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReportFileRepository ReportFileRepository => _reportFileRepository ??= new ReportFileRepository(_dbContext);

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
