using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkBot.Data.Models
{
    public class UserMemory
    {
        public int Id { get; set; }
        public long UserID { get; set; }
        public string Memory { get; set; }
    }
}
