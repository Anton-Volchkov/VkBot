﻿// <auto-generated />

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VkBot.Data.Models;

namespace VkBot.Migrations
{
    [DbContext(typeof(MainContext))]
    internal class MainContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                b.Property<string>("СommonInfo");

                b.HasKey("Id");

                b.ToTable("Commons");
            });

            modelBuilder.Entity("VkBot.Data.Models.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("City");

                b.Property<long?>("Vk");

                b.Property<bool>("Weather");

                b.HasKey("Id");

                b.ToTable("Users");
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