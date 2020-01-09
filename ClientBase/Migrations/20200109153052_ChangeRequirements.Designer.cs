﻿// <auto-generated />
using System;
using ClientBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ClientBase.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200109153052_ChangeRequirements")]
    partial class ChangeRequirements
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ClientBase.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsIndividual")
                        .IsRequired()
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<long>("TaxpayerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TaxpayerId")
                        .IsUnique();

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("ClientBase.Models.CompanyFounder", b =>
                {
                    b.Property<int>("FounderId")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.HasKey("FounderId", "CompanyId");

                    b.HasIndex("CompanyId");

                    b.ToTable("CompanyFounder");
                });

            modelBuilder.Entity("ClientBase.Models.Founder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Patronymic")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<long>("TaxpayerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TaxpayerId")
                        .IsUnique();

                    b.ToTable("Founders");
                });

            modelBuilder.Entity("ClientBase.Models.CompanyFounder", b =>
                {
                    b.HasOne("ClientBase.Models.Company", "Company")
                        .WithMany("CompanyFounders")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClientBase.Models.Founder", "Founder")
                        .WithMany("FounderCompanies")
                        .HasForeignKey("FounderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
