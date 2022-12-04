using ReportMicroservice.Api.DbContexts;
using ReportMicroservice.Api.Models;
using ReportMicroservice.Api.Repositories.Contracts;

namespace ReportMicroservice.Api.Repositories
{
    public class ReportFileRepository : GenericRepository<ReportFile>, IReportFileRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReportFileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
