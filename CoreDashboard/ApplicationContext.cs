using CoreDashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreDashboard
{
    /*public class ApplicationContext : IdentityDbContext<ApplicationUser>*/
    public class ApplicationContext : IdentityDbContext<User>
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

        public DbSet<Discipline> Disciplines { get; set; } = null!;
        public DbSet<PairTheme> PairThemes { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<StudyDirection> StudyDirections { get; set; } = null!;
        public DbSet<StudyGroup> StudyGroups { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<UploadedDb> UploadedDbs { get; set; } = null!;
        public DbSet<UploadedDbRecord> UploadedDbRecords { get; set; } = null!;
        public DbSet<UploadedDbResult> UploadedDbResults { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserType> UserTypes { get; set; } = null!;

        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole("admin");
            admin.NormalizedName = "ADMIN";

            var curator = new IdentityRole("curator");
            *//*user.NormalizedName = "USER";*//*

            builder.Entity<IdentityRole>().HasData(admin, curator);
        }*/
    }
}
