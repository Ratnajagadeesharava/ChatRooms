using Microsoft.EntityFrameworkCore;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace ChatApp.Database
{
    public class ApplicationDbContext:IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // creating composite key   
            base.OnModelCreating(builder);
            builder.Entity<ChatUser>().HasKey(x=>new{x.ChatId,x.UserId});
        }
    }
}