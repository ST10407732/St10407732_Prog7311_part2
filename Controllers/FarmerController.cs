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

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile imageFile)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var farmerProfile = await _context.FarmerProfiles.FirstOrDefaultAsync(f => f.UserId == user.Id);

                if (farmerProfile == null)
                {
                    TempData["ErrorMessage"] = "Farmer profile not found.";
                    return RedirectToAction("AddProduct");
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    product.ImageUrl = "/images/products/" + uniqueFileName;
                }

                product.FarmerProfileId = farmerProfile.Id;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Product added successfully!";
                return RedirectToAction("AddProduct");
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                TempData["ErrorMessage"] = "An error occurred while adding the product. Please try again.";
                return RedirectToAction("AddProduct");
            }
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
