using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Models;

namespace PaymentGateway.Data
{
    public class GatewayDbContext : DbContext
    {
        public GatewayDbContext(DbContextOptions options): base(options)
        {
            
        }

        public GatewayDbContext()
        {
            
        }
        
        
        public DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=PaymentGateway.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}