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

            // Seed Data ===========================================
            builder.Entity<Metal>().HasData(MetalSeed.Data);
            builder.Entity<Karat>().HasData(KaratSeed.Data);
        }
    }
}
