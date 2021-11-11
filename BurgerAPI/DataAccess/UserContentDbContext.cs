using Microsoft.EntityFrameworkCore;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.DataAccess
{
    public class UserContentDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = usersContent.db");
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}