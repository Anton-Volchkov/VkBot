using Microsoft.EntityFrameworkCore;

namespace VkBot.Data.Models
{
    public class TimeTableContext : DbContext
    {
        public DbSet<TimeTable> TimeTables { get; set; }

        public TimeTableContext(DbContextOptions<TimeTableContext> options) : base(options)
        {
            //TODO: это не нужно вообще здесь
            //Database.EnsureCreated();
        }

    }
}