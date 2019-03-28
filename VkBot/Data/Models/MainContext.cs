using Microsoft.EntityFrameworkCore;

namespace VkBot.Data.Models
{
    public class MainContext : DbContext
    {
        public DbSet<TimeTable> TimeTables { get; set; }

        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            //TODO: это не нужно вообще здесь
            //Database.EnsureCreated();
        }
    }
}