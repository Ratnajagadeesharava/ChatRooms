using System.Collections.Generic;
namespace ChatApp.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatUser> ChatUsers { get; set; }
        public ChatType Type { get; set; }
        public Chat()
        {
            ChatUsers = new List<ChatUser>();
            Messages = new List<Message>();
        }
        
        
        public string RoomName { get; set; }
    }
    
}