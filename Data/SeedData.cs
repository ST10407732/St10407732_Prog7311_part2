using AgriConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;
public static class DbSeeder
{
    public static async Task SeedData(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Create roles if they don't exist
        if (!await roleManager.RoleExistsAsync("Employee"))
            await roleManager.CreateAsync(new IdentityRole("Employee"));

        if (!await roleManager.RoleExistsAsync("Farmer"))
            await roleManager.CreateAsync(new IdentityRole("Farmer"));

        // Create default Employee
        var employeeEmail = "employee@agri.com";
        var existingUser = await userManager.FindByEmailAsync(employeeEmail);
        if (existingUser == null)
        {
            var user = new User
            {
                UserName = employeeEmail,
                Email = employeeEmail,
                FullName = "Admin Employee"
            };

            await userManager.CreateAsync(user, "Employee123!");
            await userManager.AddToRoleAsync(user, "Employee");
        }
    }
}
