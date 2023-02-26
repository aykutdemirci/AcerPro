using AcerPro.Dto.Models;
using System.Data.Entity;

namespace AcerPro.Database.Context
{
    public class AcerProDbContext : DbContext
    {
        public AcerProDbContext() : base("AcerProDbConnectionString")
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Log> Logs { get; set; } 

        public DbSet<Application> Applications { get; set; }

        public DbSet<Error> Errors { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
