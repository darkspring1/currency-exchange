using Dal.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Dal
{
    public class ExchangeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ExchangeHistory> ExchangeHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: move to config
            optionsBuilder
                .UseNpgsql(@"Host=localhost;Username=root;Password=root;Database=currency_exchange")
                .UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder
                .Entity<Account>()
                .HasKey(x => new { x.UserId, x.CurrencyId });
        }
    }
}
