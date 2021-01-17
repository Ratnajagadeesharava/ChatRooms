using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Hubs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ChatApp.Database;
using ChatApp.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(string message,int chatId,string roomName)
        {
            Console.WriteLine(message+" "+chatId+" "+ roomName);
             var chat = await _db.Chats.Include(x => x.Messages).FirstOrDefaultAsync(x=>x.Id==chatId);
            var MessageObj = new Message{
                Text    = message,
                Name    =User.Identity.Name,
                TimeStamp = DateTime.Now
            };
            chat.Messages.Add(MessageObj);
            _db.Chats.Update(chat);
            await _db.SaveChangesAsync();
            await _chatHub.Clients
                            .All
                            .SendAsync("recieveMethodFromServer",MessageObj);
                            
                            //RecieveMethodFromServer is a method on client
            //SendAsync can take methodName and Object(can be any type )of maximum 10 and can be null
            // await _chatHub.Clients.All.SendAsync("MethodName",MessageObj);
            return Ok();
        }
    }
}