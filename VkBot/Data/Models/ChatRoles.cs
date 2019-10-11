using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Bot.Help;

namespace VkBot.Data.Models
{
    public class ChatRoles
    {
        public int Id { get; set; }
        public long? UserID  { get; set; }
        public long? ChatID { get; set; }
        public Roles UserRole { get; set; }
    }
}
