using Microsoft.EntityFrameworkCore;

namespace VkBot.Data.Models
{
    public class MainContext : DbContext
    {
        public DbSet<Common> Commons { get; set; }
        public DbSet<UserMemory> Memories { get; set; }

        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            //TODO: это не нужно вообще здесь
            //Database.EnsureCreated();
        }
    }
}