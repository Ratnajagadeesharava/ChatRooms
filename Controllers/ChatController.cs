using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Hubs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ChatApp.Database;
using ChatApp.Models;
using System;

namespace Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController:Controller
    {
        
        private IHubContext<ChatHub> _chatHub { get; }
        private ApplicationDbContext  _db;
        //constructor to initialize ChatHub and DataBase
        public ChatController(IHubContext<ChatHub> chatHub,ApplicationDbContext db)
        {
            _chatHub = chatHub;
            _db=db;
            
        }
        //join Room Method
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
        public async Task<IActionResult> SendMessage(string message,string chatId,string roomName)
        {
            var MessageObj = new Message{
                
                Text    = message,
                Name    =User.Identity.Name,
                TimeStamp = DateTime.Now
            };
            _db.Messages.Add(MessageObj);
            await _db.SaveChangesAsync();
            await _chatHub.Clients
                            .Group(roomName)
                            .SendAsync("RecieveMethodFromServer",MessageObj);
                            //RecieveMethodFromServer is a method on client

            return Ok();
        }
    }
}