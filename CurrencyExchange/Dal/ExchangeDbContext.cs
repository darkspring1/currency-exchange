using Microsoft.EntityFrameworkCore;

namespace Dal
{
    public class ExchangeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: move to config
            optionsBuilder.UseNpgsql(@"Host=localhost;Username=root;Password=root;Database=currency_exchange");
        }
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
