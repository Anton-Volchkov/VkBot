namespace VkBot.Domain.Models
{
    public class BlackList
    {
        public int Id { get; set; }
        public long UserVkId  { get; set; }
        public long ChatVkId { get; set; }
    }
}
