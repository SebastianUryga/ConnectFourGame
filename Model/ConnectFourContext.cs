using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameConnectFour.Model
{
    public class ConnectFourContext : DbContext
    {
        public DbSet<GameHistory> GameHistories { get; set; }

        public ConnectFourContext() { }

        public ConnectFourContext(DbContextOptions<ConnectFourContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                   .AddJsonFile("appsettings.json")
                   .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
