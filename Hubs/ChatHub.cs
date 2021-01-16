using Microsoft.AspNetCore.SignalR;
// using System.Threading.Tasks;
namespace ChatApp.Hubs
{
    public class ChatHub:Hub
    {
        public string GetConnectionId()
        {
                return Context.ConnectionId;
        }
        
    }
}