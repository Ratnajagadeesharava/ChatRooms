using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Hubs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ChatApp.Database;
using ChatApp.Models;

namespace Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController:Controller
    {
        
        public IHubContext<ChatHub> _chatHub { get; }
        private ApplicationDbContext  _db;
        public ChatController(IHubContext<ChatHub> chatHub,ApplicationDbContext db)
        {
            _chatHub = chatHub;
            _db=db;
            
        }
        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinRoom(string connectionId,string roomName){
            await _chatHub.Groups.AddToGroupAsync(connectionId,roomName);
            return Ok();
        }
        [HttpPost("[action]/{connectionId}/{roomName}")]
         public async Task<IActionResult> LeaveRoom(string connectionId,string roomName){
            await _chatHub.Groups.RemoveFromGroupAsync(connectionId,roomName);
            return Ok();
        }
        public async Task<IActionResult> SendMessage(string Message,string roomName)
        {
            await _chatHub.Clients.Group(roomName).SendAsync();
            return Ok();
        }
    }
}