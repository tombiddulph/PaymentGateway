using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Infrastructure
{
    public sealed class PaymentGatewayDbContext : DbContext
    {
        public PaymentGatewayDbContext(DbContextOptions options) : base(options)
        {
            if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                Database.Migrate();
            }
        }

        public PaymentGatewayDbContext()
        {
        }


        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.Options.Extensions.Any(x => x is InMemoryOptionsExtension))
            {
                //horrible hack
                return;
            }

            optionsBuilder.UseSqlite("Data Source=PaymentGateway.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //this is for illustrative purposes
            modelBuilder.Entity<Merchant>().HasData(
                new Merchant
                {
                    Id = Guid.Parse("13751f4d-455b-427e-907f-f0bcc60b869e"),
                    Name = "Computer shop inc"
                },
                new Merchant
                {
                    Id = Guid.Parse("8d5b2c2b-9b8a-4ad1-9286-a8107ae7f345"),
                    Name = "Groceries r us"
                });

            modelBuilder.Entity<Card>()
                .HasMany(c => c.Transactions)
                .WithOne(x => x.Card);

            base.OnModelCreating(modelBuilder);
        }
    }
}