﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VkBot.Data.Models;

namespace VkBot.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20190410190802_BigUpdateDataBase")]
    partial class BigUpdateDataBase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("VkBot.Data.Models.Common", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("commonInfo");

                    b.HasKey("Id");

                    b.ToTable("Commons");
                });

            modelBuilder.Entity("VkBot.Data.Models.UserMemory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Memory");

                    b.Property<long>("UserID");

                    b.HasKey("Id");

                    b.ToTable("Memories");
                });
#pragma warning restore 612, 618
        }
    }
}
