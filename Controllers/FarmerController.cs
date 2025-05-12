using AgriConnect.Data;
using AgriConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AgriConnect.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class FarmerController : Controller
    {
        private readonly AgriDbContext _context;
        private readonly UserManager<User> _userManager;

        public FarmerController(AgriDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Farmer/AddProduct
        public IActionResult AddProduct()
        {
            return View();
        }

        // POST: Farmer/AddProduct
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
           // if (!ModelState.IsValid) return View(product);

            var user = await _userManager.GetUserAsync(User);
            var farmerProfile = await _context.FarmerProfiles.FirstOrDefaultAsync(f => f.UserId == user.Id);

            if (farmerProfile == null)
                return NotFound("Farmer profile not found.");

            product.FarmerProfileId = farmerProfile.Id;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyProducts");
        }

        // GET: Farmer/MyProducts
        public async Task<IActionResult> MyProducts()
        {
            var user = await _userManager.GetUserAsync(User);
            var farmerProfile = await _context.FarmerProfiles
                .Include(f => f.Products)
                .FirstOrDefaultAsync(f => f.UserId == user.Id);

            if (farmerProfile == null)
                return NotFound("Farmer profile not found.");

            return View(farmerProfile.Products);
        }
    }
}
