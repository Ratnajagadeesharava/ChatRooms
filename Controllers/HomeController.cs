using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ChatApp.Models;
using ChatApp.Database;
using System;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatApp.Controllers
{
   public class HomeController : Controller
   {
       static int i=0;
       private ApplicationDbContext _db;
       public HomeController(ApplicationDbContext db)
       {
           _db = db;
       }

       public IActionResult Index()
       {
            if(HttpContext.User.Identity.IsAuthenticated){
                Console.Write(User.Identity.Name);
                return View();
            }
            else{
                return RedirectToAction("Login","Account");
            }
       }
       [HttpPost]
       public async Task<IActionResult> CreateRoom(Chat chat){
        //    Console.WriteLine(chat.RoomName+i +":roomname");
        //    i++;

           chat.Type= ChatType.ROOM;
           chat.ChatUsers.Add(new ChatUser{
            userRole = UserRole.ADMIN,
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
            
           });
           await _db.Chats.AddAsync(chat);
           
           await _db.SaveChangesAsync();
           return RedirectToAction("Index");
       }
       [HttpPost]
       public async Task<IActionResult> JoinChat(int Id){
           var chat =await _db.Chats.FirstOrDefaultAsync(x => x.Id == Id);
           if(chat == null){
                return RedirectToAction("JoinChat");
           }
           
           chat.ChatUsers.Add(new ChatUser{
               UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
               userRole = UserRole.NORMALUSER,
               ChatId = Id
           });
           await _db.SaveChangesAsync();
           return RedirectToAction("ChatRoom",new {Id= Id});
       }
       [HttpGet("{Id}")]
       public async Task<IActionResult> ChatRoom(int id){
           var chat =await _db.Chats.Include(x => x.Messages).FirstOrDefaultAsync(x=>x.Id==id);
           return View(chat);
       }
       [HttpPost]
       public async Task<IActionResult> SendMessage(int ChatId,string ChatMessage){
           var chat = await _db.Chats.Include(x => x.Messages).FirstOrDefaultAsync(x=>x.Id==ChatId);
           chat.Messages.Add(new Message{
            Text = ChatMessage,
            Name = User.Identity.Name,
            TimeStamp = DateTime.Now
           });
           _db.Chats.Update(chat);
           _db.SaveChanges();
           
           return RedirectToAction("ChatRoom",new {id=ChatId});
       }
   }
   public class MessageDto{
       public int ChatId;
       public string ChatMessage;
   }
}