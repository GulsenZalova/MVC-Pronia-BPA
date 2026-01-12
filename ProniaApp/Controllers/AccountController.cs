using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.Models;
using ProniaApp.Utilities.Enums;
using ProniaApp.ViewModels;

namespace ProniaApp.Controllers
{

    public class AccountController : Controller
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly SignInManager<AppUser> _signInManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]

        public async Task<ActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            AppUser user = new AppUser()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }

            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction(nameof(HomeController.Index), "Home");


        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<ActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginVM loginVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Name == loginVM.NameOrEmail || u.Email == loginVM.NameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "Email Or Name invalid");
                return View();
            }
            // AppUser user= await _userManager.FindByNameAsync(loginVM.NameOrEmail);
            // if (user == null)
            // {
            //     user=await _userManager.FindByEmailAsync(loginVM.NameOrEmail);
            //     if(user == null)
            //     {
            //         ModelState.AddModelError("","Email Or Name invalid");
            //         return View();
            //     }
            // }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersisted, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "you are locked");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "login failed");
                return View();
            }


            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> CreateRole()
        {
            // await _roleManager.CreateAsync(new IdentityRole("Admin"));
            // await _roleManager.CreateAsync(new IdentityRole("User"));
            // await _roleManager.CreateAsync(new IdentityRole("Moderator"));

            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole{Name=role.ToString()});
                }
            }

            return RedirectToAction("Index", "Home");
        }

    }
}