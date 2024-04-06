using CoreDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreDashboard
{
    public class ApplicationContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ApplicationContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("CoreDashboardDatabase"));
        }

        public DbSet<EducationalRecord> EducationalRecords { get; set; } = null!;
    }
}
