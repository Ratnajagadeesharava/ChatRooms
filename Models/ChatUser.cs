namespace ChatApp.Models
{
    public class ChatUser
    {
        public User user { get; set; }
        public string UserId { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public UserRole userRole { get; set; }
    }
}