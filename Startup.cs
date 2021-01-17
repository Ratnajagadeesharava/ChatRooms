using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatApp.Database;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.AspNetCore.Identity;
using ChatApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Hubs;


namespace ChatApp
{
    public class Startup
    {
        private IConfiguration _config;   
        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
           // using Microsoft.EntityFrameworkCore;
           services.AddDbContext<ApplicationDbContext>(options =>{
               options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
           });
              
           services.AddIdentity<User,IdentityRole>(
               options =>{
                   options.Password.RequireDigit = false;
                   options.Password.RequiredLength=6;
                   options.Password.RequireLowercase=false;
                   options.Password.RequireNonAlphanumeric=false;
                   options.Password.RequireUppercase=false;

               }
           ).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
           services.AddSignalR();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
               if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapHub<ChatHub>("/chatHub");
            });
            
            
        

          
        }
    }
}
