using System;
using System.Collections.Generic;
using System.Text;
using jpfc.Data.SeedData;
using jpfc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace jpfc.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<AccessCode> AccessCode { get; set; }
        public DbSet<Price> Price { get; set; }
        public DbSet<Karat> Karat { get; set; }
        public DbSet<Metal> Metal { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ClientBelonging> ClientBelonging { get; set; }
        public DbSet<IdentificationDocument> IdentificationDocument { get; set; }
        public DbSet<ClientReceipt> ClientReceipt { get; set; }
        public DbSet<ClientIdentification> ClientIdentification { get; set; }
        public DbSet<MortgageRate> MortgageRate { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<AccessCode>(entity =>
            {
                entity.HasKey(a => a.AccessCodeId);
            });

            builder.Entity<Metal>(entity =>
            {
                entity.HasKey(a => a.MetalId);
                entity.Property(e => e.Name).HasMaxLength(255);
            });

            builder.Entity<Karat>(entity =>
            {
                entity.HasKey(a => a.KaratId);
                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(e => e.Metal)
                .WithMany(m => m.Karat)
                .HasForeignKey(e => e.MetalId);
            });

            builder.Entity<Price>(entity =>
            {
                entity.HasKey(a => a.PriceId);

                entity.HasOne(e => e.Metal)
                .WithMany(a => a.Prices)
                .HasForeignKey(e => e.MetalId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Karat)
                .WithMany(a => a.Prices)
                .HasForeignKey(e => e.KaratId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<IdentificationDocument>(entity =>
            {
                entity.HasKey(e => e.IdentificationDocumentId);
            });

            builder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ClientId);
                entity.Property(e => e.ReferenceNumber).HasMaxLength(50);
                entity.Property(e => e.EmailAddress).HasMaxLength(100);
            });

            builder.Entity<ClientBelonging>(entity =>
            {
                entity.HasKey(e => e.ClientBelongingId);

                entity.HasOne(e => e.ClientReceipt)
                .WithMany(c => c.ClientBelongings)
                .HasForeignKey(e => e.ClientReceiptId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Metal)
                .WithMany()
                .HasForeignKey(e => e.MetalId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Karat)
                .WithMany()
                .HasForeignKey(e => e.KaratId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ClientReceipt>(entity =>
            {
                entity.HasKey(e => e.ClientReceiptId);
                entity.Property(e => e.ReceiptNumber).HasMaxLength(255);

                entity.HasOne(e => e.Client)
                .WithMany(c => c.Receipts)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ClientIdentification)
               .WithMany(i => i.ClientReceipts)
               .HasForeignKey(e => e.ClientIdentificationId)
               .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ClientIdentification>(entity =>
            {
                entity.HasKey(e => e.ClientIdentificationId);

                entity.HasOne(e => e.Client)
                .WithMany(c => c.Identifications)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.IdentificationDocument)
                .WithMany()
                .HasForeignKey(e => e.IdentificationDocumentId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<MortgageRate>(entity =>
            {
                entity.HasKey(e => e.MortgageRateId);
            });

            // Seed Data ===========================================
            builder.Entity<Metal>().HasData(MetalSeed.Data);
            builder.Entity<Karat>().HasData(KaratSeed.Data);
            builder.Entity<IdentificationDocument>().HasData(IdentificationDocumentSeed.Data);
        }
    }
}
