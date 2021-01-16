using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using  ChatApp.Models;
using System.Threading.Tasks;
namespace Controllers
{
    public class AccountController:Controller
    {
        /// <summary>
        /// SignInManagert takes TUser as generic which in asp.netcore is a default method which manages signin
        /// </summary>
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        public AccountController(SignInManager<User> signInManager,UserManager<User> userManager)
        {
            _signInManager  =   signInManager;
            _userManager    =   userManager;
        }
        [HttpGet]
        public IActionResult Login(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userName ,string password){
            /// <summary>
            /// First we need to check for the user exists with the username by userManager (present is aspnetcore.Identity)
            /// then we will verify using SignInManager
            /// </summary>
            var user = await  _userManager.FindByNameAsync(userName);
            if(user!=null){
                var result = await _signInManager.PasswordSignInAsync(user,password,false,false);
                if(result.Succeeded) return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Login","Account");
        }
        [HttpGet]
        public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(string userName,string password){
            User user  = new User{
                UserName = userName
            };
            var result = await _userManager.CreateAsync(user,password);
            if(result.Succeeded){
              await _signInManager.SignInAsync(user, false);
              return RedirectToAction("Index","Home");
            }
            return View();
        }
        public async Task<IActionResult> Logout(){
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}