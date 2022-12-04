using Microsoft.EntityFrameworkCore;
using ReportMicroservice.Api.Models;

namespace ReportMicroservice.Api.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ReportFile> ReportFiles { get; set; }
    }
}
