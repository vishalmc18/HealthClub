using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModel;
using System.Net.WebSockets;

namespace RunGroupWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var response = new Loginviewmodel();

            return View(response);
           // return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Loginviewmodel loginviewmodel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginviewmodel);
            }
            var user = await _userManager.FindByEmailAsync(loginviewmodel.EmailAddress);
            if (user != null)
            {
                //user found
                var passwordcheck = await _userManager.CheckPasswordAsync(user,loginviewmodel.Password);
                if (passwordcheck)
                {
                    //correct password
                    var result = await _signInManager.PasswordSignInAsync(user, loginviewmodel.Password,false,false);
                    if(result.Succeeded)
                    {
                        return Json(new { result = "success" });
                        //return RedirectToAction("Index", "Race");
                    }
                }
                //wrong password
                TempData["ErrorMessage"] = "Wrong password entered, Try again";
                //return View(loginviewmodel);
                return BadRequest("Wrong password entered");
            }
            //user not found
            TempData["ErrorMessage"] = "User not found, please enter correct UserId";
            //return View(loginviewmodel);
            return  BadRequest("User not found in system");

        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerviewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerviewmodel);
            }
            var user = await _userManager.FindByEmailAsync(registerviewmodel.EmailAddress);
            //user found with this email in the database
            if (user != null)
            {
                TempData["Error"] = "User already exist with this Email Address";
                return View(registerviewmodel);
            }
            var newuser = new AppUser()
            {
                Email = registerviewmodel.EmailAddress,
                UserName = registerviewmodel.EmailAddress,

            };
            var newuserResponse = await _userManager.CreateAsync(newuser,registerviewmodel.Password);
            if(newuserResponse.Succeeded)
            {
                //for register as a admin
                //await _userManager.AddToRoleAsync(newuser, UserRoles.Admin);
                //for register as user
                await _userManager.AddToRoleAsync(newuser, UserRoles.User);
            }
            return RedirectToAction("Index","Race");

        }


        //[HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Race");
        }
    }
}
