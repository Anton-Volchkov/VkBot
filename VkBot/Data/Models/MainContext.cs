using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace VkBot.Data.Models
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            //TODO: это не нужно вообще здесь
            //Database.EnsureCreated();
        }

        public DbSet<Common> Commons { get; set; }
        public DbSet<UserMemory> Memories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TimeTable> TimeTable { get; set; }


        public User[] GetUsers()
        {
            return Users.AsNoTracking().ToArray();
        }

        public User[] GetWeatherUsers()
        {
            return Users.AsNoTracking().Where(x => x.Weather && x.City != "").ToArray();
        }

        public User[] GetScheduleUsers()
        {
            return Users.AsNoTracking().Where(x => x.Group != "").ToArray();
        }
    }
}