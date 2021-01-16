using Microsoft.AspNetCore.Mvc;
using ChatApp.Database;
using ChatApp.Models;
using System.Linq;
using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace ChatApp.ViewComponents
{
    public class RoomViewComponent :ViewComponent//ViewComponent is similar to Components in Angular
    {
        private ApplicationDbContext _db;
        public RoomViewComponent(ApplicationDbContext db)
        {
            // Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Room Componenet has been called");
            _db = db;
        }
        public IViewComponentResult Invoke(){
            var chats = new List<Chat>();
            if(HttpContext.User.Identity.IsAuthenticated){
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                // var chats = _db.Chats.ToList();//ToList is present in System.Linq
                 chats  = _db.ChatUsers
                            .Include(x => x.Chat)
                            .Where(x => x.UserId==userId)
                            .Select(x=>x.Chat)
                            .ToList();
                
               
            }
            return View(chats);
            
            
        }
    }
}