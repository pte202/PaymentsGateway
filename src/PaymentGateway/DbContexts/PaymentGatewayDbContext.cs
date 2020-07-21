using Microsoft.EntityFrameworkCore;
using PaymentGateway.Entities;
using System;

namespace PaymentGateway.DbContexts
{
    public class PaymentGatewayDbContext : DbContext
    {
        public PaymentGatewayDbContext(DbContextOptions<PaymentGatewayDbContext> options) : base(options) { }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentCard> PaymentCards { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<PaymentCardIssuer> PaymentCardIssuers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>(e =>
            {
                e.ToTable("Payments");
                e.HasKey(e => e.Id);
                e.HasIndex(e => e.Identifier).IsUnique();
                e.Property(e => e.Date).HasColumnType("date");
                e.HasData(
                    new Payment
                    {
                        Id = 1,
                        Identifier = "63d46c20-61bf-4c47-bfdc-6f08bc217406",
                        Date = new DateTime(2001, 11, 01),
                        Status = "success",
                        Amount = 1243,
                        CurrencyId = 1,
                        Method = "Card",
                        PaymentCardId = 1
                    }
                );
            });

            modelBuilder.Entity<PaymentCard>(e =>
            {
                e.ToTable("PaymentCards");
                e.HasKey(e => e.Id);
                e.HasIndex(e => e.Number).IsUnique();
                e.Property(e => e.ExpiryDate).HasColumnType("date");
                e.HasData(
                    new PaymentCard
                    {
                        Id = 1,
                        Number = "4111111111111111",
                        ExpiryDate = new DateTime(2020, 06, 30),
                        CardIssuerId = 1
                    }
                );
            });

            modelBuilder.Entity<Currency>(e =>
            {
                e.ToTable("Currencies");
                e.HasKey(e => e.Id);
                e.HasIndex(e => e.Code).IsUnique();
                e.HasData(
                    new Currency
                    {
                        Id = 1,
                        Name = "Pound sterling",
                        Code = "GBP"
                    },
                    new Currency
                    {
                        Id = 2,
                        Name = "United States Dollar",
                        Code = "USD"
                    }
                );
            });

            modelBuilder.Entity<PaymentCardIssuer>(e =>
            {
                e.ToTable("PaymentCardIssuers");
                e.HasKey(e => e.Id);
                e.HasData(
                    new PaymentCardIssuer
                    {
                       Id = 1,
                       Name = "AmericanExpress", 
                       Pattern = @"^3[47][0-9]{5,}$"
                    },
                    new PaymentCardIssuer 
                    {
                        Id = 2,
                        Name = "Visa", 
                        Pattern = @"^4[0-9]{6,}$" 
                    },
                    new PaymentCardIssuer
                    {
                        Id = 3,
                        Name = "Mastercard", 
                        Pattern = @"^5[1-5][0-9]{5,}|222[1-9][0-9]{3,}|22[3-9][0-9]{4,}|2[3-6][0-9]{5,}|27[01][0-9]{4,}|2720[0-9]{3,}$" 
                    }
                );
            });

        }
    }
}
