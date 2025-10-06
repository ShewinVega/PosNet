using Microsoft.EntityFrameworkCore;

namespace PosNet.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Permissions> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>()
                .HasMany(per => per.Permissions)
                .WithMany(role => role.Roles);

            base.OnModelCreating(modelBuilder);
        }
    }
}
