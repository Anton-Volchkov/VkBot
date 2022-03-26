using System.Linq;
using Microsoft.EntityFrameworkCore;
using VkBot.Domain.Models;

namespace VkBot.Domain
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
         
        }

        public DbSet<Common> Commons { get; set; }
        public DbSet<UserMemory> Memories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TimeTable> TimeTable { get; set; }
        public DbSet<ChatRoles> ChatRoles { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<BlackList> BlackList { get; set; }

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
            return Users.AsNoTracking().Where(x => x.Group != "" && x.Group != null).ToArray();
        }
    }
}