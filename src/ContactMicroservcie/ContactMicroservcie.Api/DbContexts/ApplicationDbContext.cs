using ContactMicroservcie.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactMicroservcie.Api.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
