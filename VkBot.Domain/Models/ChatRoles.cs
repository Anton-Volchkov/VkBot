namespace VkBot.Domain.Models
{
    public class ChatRoles
    {
        public int Id { get; set; }
        public long? UserVkID { get; set; }
        public long? ChatVkID { get; set; }
        public int AmountChatMessages { get; set; }
        public string Status { get; set; }
        public byte Rebuke { get; set; }
        public Roles UserRole { get; set; }
    }
}