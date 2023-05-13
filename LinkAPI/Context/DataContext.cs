using LinkAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkAPI.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Link> Links { get; set; }
        public DbSet<User> Users { get; set; }
        public DataContext(DbContextOptions options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(user => user.Username);
        }
    }
}
