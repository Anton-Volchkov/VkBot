﻿using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace VkBot.Data.Models
{
    public class MainContext : DbContext
    {
        public DbSet<Common> Commons { get; set; }
        public DbSet<UserMemory> Memories { get; set; }
        public DbSet<User> Users { get; set; }
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            //TODO: это не нужно вообще здесь
            //Database.EnsureCreated();
        }

        public User[] GetUsers()
        {
            return Users.AsNoTracking().ToArray();
        }

        public User[] GetWeatherUsers()
        {
            return Users.AsNoTracking().Where(x => x.Weather && x.City != "").ToArray();
        }

    }
}