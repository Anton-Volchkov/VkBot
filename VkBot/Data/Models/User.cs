namespace VkBot.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        public long? Vk { get; set; }
        public bool Weather { get; set; }
        public string City { get; set; }
    }
}