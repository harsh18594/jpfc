﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using jpfc.Data;

namespace jpfc.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("jpfc.Models.AccessCode", b =>
                {
                    b.Property<Guid>("AccessCodeId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Hash");

                    b.Property<string>("PlainTextCode");

                    b.Property<string>("Salt");

                    b.HasKey("AccessCodeId");

                    b.ToTable("AccessCode");
                });

            modelBuilder.Entity("jpfc.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("jpfc.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(500);

                    b.Property<string>("AuditUserId");

                    b.Property<DateTime?>("AuditUtc");

                    b.Property<string>("ContactNumber")
                        .HasMaxLength(20);

                    b.Property<string>("CreatedUserId");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<DateTime>("Date");

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(100);

                    b.Property<string>("Name");

                    b.Property<string>("ReferenceNumber")
                        .HasMaxLength(50);

                    b.HasKey("ClientId");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("jpfc.Models.ClientBelonging", b =>
                {
                    b.Property<int>("ClientBelongingId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuditUserId");

                    b.Property<DateTime?>("AuditUtc");

                    b.Property<int>("ClientReceiptId");

                    b.Property<string>("CreatedUserId");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<decimal?>("FinalPrice");

                    b.Property<decimal?>("ItemPrice");

                    b.Property<decimal?>("ItemWeight");

                    b.Property<Guid?>("KaratId");

                    b.Property<string>("KaratOther");

                    b.Property<Guid?>("MetalId");

                    b.Property<string>("MetalOther");

                    b.Property<decimal?>("ReplacementValue");

                    b.Property<string>("TransactionAction");

                    b.HasKey("ClientBelongingId");

                    b.HasIndex("ClientReceiptId");

                    b.HasIndex("KaratId");

                    b.HasIndex("MetalId");

                    b.ToTable("ClientBelonging");
                });

            modelBuilder.Entity("jpfc.Models.ClientIdentification", b =>
                {
                    b.Property<int>("ClientIdentificationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuditUserId");

                    b.Property<DateTime?>("AuditUtc");

                    b.Property<int>("ClientId");

                    b.Property<string>("CreatedUserId");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<Guid?>("IdentificationDocumentId");

                    b.Property<string>("IdentificationDocumentNumber")
                        .HasMaxLength(255);

                    b.HasKey("ClientIdentificationId");

                    b.HasIndex("ClientId");

                    b.HasIndex("IdentificationDocumentId");

                    b.ToTable("ClientIdentification");
                });

            modelBuilder.Entity("jpfc.Models.ClientReceipt", b =>
                {
                    b.Property<int>("ClientReceiptId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuditUserId");

                    b.Property<DateTime?>("AuditUtc");

                    b.Property<int>("ClientId");

                    b.Property<int>("ClientIdentificationId");

                    b.Property<string>("CreatedUserId");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<DateTime>("Date");

                    b.Property<string>("ReceiptNumber")
                        .HasMaxLength(255);

                    b.HasKey("ClientReceiptId");

                    b.HasIndex("ClientId");

                    b.HasIndex("ClientIdentificationId");

                    b.ToTable("ClientReceipt");
                });

            modelBuilder.Entity("jpfc.Models.IdentificationDocument", b =>
                {
                    b.Property<Guid>("IdentificationDocumentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int?>("SortOrder");

                    b.HasKey("IdentificationDocumentId");

                    b.ToTable("IdentificationDocument");

                    b.HasData(
                        new { IdentificationDocumentId = new Guid("78faffc2-601a-4a06-b7ef-0e3b5a7f8b96"), Name = "Driver's License" },
                        new { IdentificationDocumentId = new Guid("4eed5d73-e2b9-406f-979f-7c124813eef3"), Name = "Passport" },
                        new { IdentificationDocumentId = new Guid("5bb91152-bee1-4afb-8c0b-aa95e6c4a9b5"), Name = "PR Card" }
                    );
                });

            modelBuilder.Entity("jpfc.Models.Karat", b =>
                {
                    b.Property<Guid>("KaratId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("InActive");

                    b.Property<Guid>("MetalId");

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.HasKey("KaratId");

                    b.HasIndex("MetalId");

                    b.ToTable("Karat");

                    b.HasData(
                        new { KaratId = new Guid("f47e14cb-9fc9-4aaf-9f2d-a64fc6a15c2f"), InActive = false, MetalId = new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"), Name = "9K" },
                        new { KaratId = new Guid("0bab0d22-f831-4b6c-b177-c623ba4bf5b9"), InActive = false, MetalId = new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"), Name = "10K" },
                        new { KaratId = new Guid("775d10c4-a955-4039-aaf6-16a80b0759f7"), InActive = false, MetalId = new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"), Name = "14K" },
                        new { KaratId = new Guid("4aaf69a9-7bb0-4f70-956d-4f55cd98fe1e"), InActive = false, MetalId = new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"), Name = "18K" },
                        new { KaratId = new Guid("4da2d061-e089-4c8d-bfa4-534a301e0c87"), InActive = false, MetalId = new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"), Name = "22K" },
                        new { KaratId = new Guid("d9fb756f-933d-4cfa-9dc3-76714b84b256"), InActive = false, MetalId = new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"), Name = "24K" },
                        new { KaratId = new Guid("06ff5e11-00cb-4cab-ae61-8237a13ae60f"), InActive = false, MetalId = new Guid("b0fd2523-63a0-4cf3-940c-627617b3196f"), Name = "950" },
                        new { KaratId = new Guid("ef76a7fe-0d8d-4814-83df-47a4a035d703"), InActive = false, MetalId = new Guid("2a883efe-13fa-4a50-ad2b-ae49b034c8b0"), Name = "925" }
                    );
                });

            modelBuilder.Entity("jpfc.Models.Metal", b =>
                {
                    b.Property<Guid>("MetalId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("InActive");

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.HasKey("MetalId");

                    b.ToTable("Metal");

                    b.HasData(
                        new { MetalId = new Guid("807f52d1-8f65-4f91-8408-3c5a04d830dd"), InActive = false, Name = "Gold" },
                        new { MetalId = new Guid("2a883efe-13fa-4a50-ad2b-ae49b034c8b0"), InActive = false, Name = "Silver" },
                        new { MetalId = new Guid("b0fd2523-63a0-4cf3-940c-627617b3196f"), InActive = false, Name = "Platinum" }
                    );
                });

            modelBuilder.Entity("jpfc.Models.Price", b =>
                {
                    b.Property<int>("PriceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuditUserId");

                    b.Property<DateTime?>("AuditUtc");

                    b.Property<decimal?>("BuyPrice");

                    b.Property<string>("CreatedUserId");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<DateTime>("Date");

                    b.Property<Guid?>("KaratId");

                    b.Property<decimal?>("LoanPrice");

                    b.Property<decimal?>("LoanPricePercent");

                    b.Property<Guid>("MetalId");

                    b.Property<bool>("PerOunce");

                    b.Property<decimal?>("SellPrice");

                    b.HasKey("PriceId");

                    b.HasIndex("KaratId");

                    b.HasIndex("MetalId");

                    b.ToTable("Price");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("jpfc.Models.ClientBelonging", b =>
                {
                    b.HasOne("jpfc.Models.ClientReceipt", "ClientReceipt")
                        .WithMany("ClientBelongings")
                        .HasForeignKey("ClientReceiptId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("jpfc.Models.Karat", "Karat")
                        .WithMany()
                        .HasForeignKey("KaratId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("jpfc.Models.Metal", "Metal")
                        .WithMany()
                        .HasForeignKey("MetalId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("jpfc.Models.ClientIdentification", b =>
                {
                    b.HasOne("jpfc.Models.Client", "Client")
                        .WithMany("Identifications")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("jpfc.Models.IdentificationDocument", "IdentificationDocument")
                        .WithMany()
                        .HasForeignKey("IdentificationDocumentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("jpfc.Models.ClientReceipt", b =>
                {
                    b.HasOne("jpfc.Models.Client", "Client")
                        .WithMany("Receipts")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("jpfc.Models.ClientIdentification", "ClientIdentification")
                        .WithMany("ClientReceipts")
                        .HasForeignKey("ClientIdentificationId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("jpfc.Models.Karat", b =>
                {
                    b.HasOne("jpfc.Models.Metal", "Metal")
                        .WithMany("Karat")
                        .HasForeignKey("MetalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("jpfc.Models.Price", b =>
                {
                    b.HasOne("jpfc.Models.Karat", "Karat")
                        .WithMany("Prices")
                        .HasForeignKey("KaratId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("jpfc.Models.Metal", "Metal")
                        .WithMany("Prices")
                        .HasForeignKey("MetalId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("jpfc.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("jpfc.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("jpfc.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("jpfc.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
