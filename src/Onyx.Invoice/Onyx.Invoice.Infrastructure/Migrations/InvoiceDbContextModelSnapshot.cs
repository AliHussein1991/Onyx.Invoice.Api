﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Onyx.Invoice.Infrastructure.Contexts;

#nullable disable

namespace Onyx.Invoice.Infrastructure.Migrations
{
    [DbContext(typeof(InvoiceDbContext))]
    partial class InvoiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Onyx.Invoice.Core.Entities.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InvoiceGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceGroupId");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("Onyx.Invoice.Core.Entities.InvoiceGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("InvoiceGroup");
                });

            modelBuilder.Entity("Onyx.Invoice.Core.Entities.Observation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GuestName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfNights")
                        .HasColumnType("int");

                    b.Property<string>("TravelAgent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("Observation");
                });

            modelBuilder.Entity("Onyx.Invoice.Core.Entities.TravelAgentInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("TotalNumberOfNights")
                        .HasColumnType("int");

                    b.Property<string>("TravelAgent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TravelAgent");
                });

            modelBuilder.Entity("Onyx.Invoice.Core.Entities.Invoice", b =>
                {
                    b.HasOne("Onyx.Invoice.Core.Entities.InvoiceGroup", "InvoiceGroup")
                        .WithMany("Invoices")
                        .HasForeignKey("InvoiceGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InvoiceGroup");
                });

            modelBuilder.Entity("Onyx.Invoice.Core.Entities.Observation", b =>
                {
                    b.HasOne("Onyx.Invoice.Core.Entities.Invoice", "Invoice")
                        .WithMany("Observations")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("Onyx.Invoice.Core.Entities.Invoice", b =>
                {
                    b.Navigation("Observations");
                });

            modelBuilder.Entity("Onyx.Invoice.Core.Entities.InvoiceGroup", b =>
                {
                    b.Navigation("Invoices");
                });
#pragma warning restore 612, 618
        }
    }
}
