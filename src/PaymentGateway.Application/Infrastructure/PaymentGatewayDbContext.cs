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
            if (Database.IsSqlite())
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


            if (Database.IsInMemory())
            {
                SeedTestData(modelBuilder);
            }
            
            base.OnModelCreating(modelBuilder);
        }

        private static void SeedTestData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>().HasData(new Card
            {
                Id = Guid.Parse("ba636903-ed0b-4801-9589-bb5c34b4211d"),
                Cvv = "123",
                Number = "1234123412341234",
                ExpiryMonth = "09",
                ExpiryYear = "30",
                HolderName = "Test User",
            });
            modelBuilder.Entity<Merchant>().HasData(new Merchant
            {
                Id = Guid.Parse("31751f4d-455b-427e-907f-f0bcc60b869e"),
                Name = "Computer shop inc"
            });

            modelBuilder.Entity<Transaction>().HasData(new Transaction
            {
                Id = Guid.Parse("59ead15a-f908-4a66-9be3-4967520c621e"),
                UserId = "Test user",
                CardId = Guid.Parse("ba636903-ed0b-4801-9589-bb5c34b4211d"),
                MerchantId = Guid.Parse("31751f4d-455b-427e-907f-f0bcc60b869e"),
                Amount = 1234.93M,
                Status = PaymentStatus.Success
            });
        }
    }
}