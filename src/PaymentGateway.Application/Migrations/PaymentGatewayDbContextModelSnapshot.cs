﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PaymentGateway.Application.Infrastructure;

namespace PaymentGateway.Application.Migrations
{
    [DbContext(typeof(PaymentGatewayDbContext))]
    partial class PaymentGatewayDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("PaymentGateway.Application.Models.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Cvv")
                        .HasColumnType("TEXT")
                        .HasMaxLength(3);

                    b.Property<string>("ExpiryMonth")
                        .HasColumnType("TEXT")
                        .HasMaxLength(2);

                    b.Property<string>("ExpiryYear")
                        .HasColumnType("TEXT")
                        .HasMaxLength(2);

                    b.Property<string>("HolderName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("Number")
                        .HasColumnType("TEXT")
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.ToTable("cards");
                });

            modelBuilder.Entity("PaymentGateway.Application.Models.Merchant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("merchants");

                    b.HasData(
                        new
                        {
                            Id = new Guid("13751f4d-455b-427e-907f-f0bcc60b869e"),
                            Name = "Computer shop inc"
                        },
                        new
                        {
                            Id = new Guid("8d5b2c2b-9b8a-4ad1-9286-a8107ae7f345"),
                            Name = "Groceries r us"
                        });
                });

            modelBuilder.Entity("PaymentGateway.Application.Models.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CardId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("PaymentGateway.Application.Models.Transaction", b =>
                {
                    b.HasOne("PaymentGateway.Application.Models.Card", "Card")
                        .WithMany("Transactions")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PaymentGateway.Application.Models.Merchant", "Merchant")
                        .WithMany()
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}