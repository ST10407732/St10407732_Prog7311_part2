using AgriConnect.Data;
using AgriConnect.Models;
using AgriConnect.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AgriConnectControllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AgriDbContext _context;

        public AccountController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            AgriDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // --------------------
        // REGISTER FARMER
        // --------------------

        [HttpGet]
        public IActionResult FarmerRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FarmerRegister(FarmerRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName

            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Ensure the "Farmer" role exists
                if (!await _roleManager.RoleExistsAsync("Farmer"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Farmer"));
                }

                await _userManager.AddToRoleAsync(user, "Farmer");

                // Create a corresponding FarmerProfile
                var profile = new FarmerProfile
                {
                    FullName = model.FullName,
                    Location = model.Location,
                   // ContactNumber = model.ContactNumber,  // Optional: could be added to ViewModel later
                    UserId = user.Id
                };

                _context.FarmerProfiles.Add(profile);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            // Display errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}
