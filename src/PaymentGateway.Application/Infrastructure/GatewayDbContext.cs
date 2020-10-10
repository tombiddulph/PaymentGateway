using Microsoft.EntityFrameworkCore;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Infrastructure
{
    public class GatewayDbContext : DbContext
    {
        public GatewayDbContext(DbContextOptions options) : base(options)
        {
        }

        public GatewayDbContext()
        {
        }

        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=PaymentGateway.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().HasOne<Merchant>().WithOne(x => x.Id);
        }
    }
}