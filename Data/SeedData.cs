using AgriConnect.Data;
using AgriConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

public class DbSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AgriDbContext _dbContext;
    private readonly ILogger<DbSeeder> _logger;

    public DbSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AgriDbContext dbContext, ILogger<DbSeeder> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedData()
    {
        // Create roles if they don't exist
        if (!await _roleManager.RoleExistsAsync("Employee"))
        {
            _logger.LogInformation("Creating 'Employee' role.");
            await _roleManager.CreateAsync(new IdentityRole("Employee"));
        }

        if (!await _roleManager.RoleExistsAsync("Farmer"))
        {
            _logger.LogInformation("Creating 'Farmer' role.");
            await _roleManager.CreateAsync(new IdentityRole("Farmer"));
        }

        // Seed the default employee user
        var employeeEmail = "employee@agri.com";
        var existingUser = await _userManager.FindByEmailAsync(employeeEmail);
        if (existingUser == null)
        {
            _logger.LogInformation("Seeding employee user.");
            var user = new User
            {
                UserName = employeeEmail,
                Email = employeeEmail,
                FullName = "Admin Employee"
            };

            var result = await _userManager.CreateAsync(user, "Employee123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Employee");
                _logger.LogInformation("Employee user seeded successfully.");
            }
            else
            {
                _logger.LogError("Failed to seed employee user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        // Seed default farmers and their products
        var farmers = new (string FullName, string Email, string FarmName, string Location)[]
        {
            ("Shawn Mavis", "farmer1@agri.com", "Green Valley Farm", "Eastern Cape"),
            ("Alice Lavender", "farmer2@agri.com", "Sunny Fields", "Western Cape"),
            ("Bob Joel", "farmer3@agri.com", "Golden Acres", "Northern Cape")
        };

        foreach (var farmer in farmers)
        {
            var farmerUser = await _userManager.FindByEmailAsync(farmer.Email);
            if (farmerUser == null)
            {
                _logger.LogInformation($"Seeding farmer user: {farmer.FullName}.");
                farmerUser = new User
                {
                    UserName = farmer.Email,
                    Email = farmer.Email,
                    FullName = farmer.FullName
                };

                var result = await _userManager.CreateAsync(farmerUser, "Farmer123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(farmerUser, "Farmer");
                    _logger.LogInformation($"Farmer {farmer.FullName} seeded successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to seed farmer {farmer.FullName}: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // Add FarmerProfile if not exists
            var existingProfile = await _dbContext.FarmerProfiles.FirstOrDefaultAsync(p => p.UserId == farmerUser.Id);
            if (existingProfile == null)
            {
                var profile = new FarmerProfile
                {
                    UserId = farmerUser.Id,
                    FullName = farmer.FarmName,
                    Location = farmer.Location
                };
                _dbContext.FarmerProfiles.Add(profile);
                await _dbContext.SaveChangesAsync();

                // Seed products for each farmer
                var products = new[]
                {
                    new Product
                    {
                        Name = "Cows",
                        Category = "Livestock",
                        ProductionDate = DateTime.Now.AddDays(-10),
                        FarmerProfileId = profile.Id
                    },
                    new Product
                    {
                        Name = "Chicken",
                        Category = "Livestock",
                        ProductionDate = DateTime.Now.AddDays(-5),
                        FarmerProfileId = profile.Id
                    },
                    new Product
                    {
                        Name = "Wheat",
                        Category = "Crops",
                        ProductionDate = DateTime.Now.AddDays(-7),
                        FarmerProfileId = profile.Id
                    }
                };

                _dbContext.Products.AddRange(products);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Farmer profile and products for {farmer.FullName} seeded successfully.");
            }
            else
            {
                _logger.LogInformation($"Farmer profile for {farmer.FullName} already exists.");
            }
        }
    }
}
