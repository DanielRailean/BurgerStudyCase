using Microsoft.EntityFrameworkCore;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.DataAccess
{
    public class UserContentDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Post> Posts { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = usersContent.db");
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}