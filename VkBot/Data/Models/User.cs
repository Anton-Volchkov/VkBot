

namespace VkBot.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        public long? Vk { get; set; }
        public bool Weather { get; set; }
        public string City { get; set; }
        public string Group { get; set; }
        public string Status { get; set; }
        public bool IsBotAdmin { get; set; }
    }
}