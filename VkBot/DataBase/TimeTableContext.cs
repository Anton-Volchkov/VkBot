using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkBot.DataBase
{
    public class TimeTableContext : DbContext
    {
        

        public DbSet<TimeTable> TimeTables { get; set; }

        public TimeTableContext(DbContextOptions<TimeTableContext> options):base(options)
        {
            Database.EnsureCreated();
        }
    }
}
