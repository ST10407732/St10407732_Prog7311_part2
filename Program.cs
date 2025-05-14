using AgriConnect.Models;
using AgriConnect.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace AgriConnect
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ Register DbContext
            builder.Services.AddDbContext<AgriDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ✅ Register Identity
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AgriDbContext>()
                .AddDefaultTokenProviders();

            // Register DbSeeder as scoped
            builder.Services.AddScoped<DbSeeder>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Apply migrations and seed data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var db = services.GetRequiredService<AgriDbContext>();
                db.Database.Migrate(); // Ensure migrations applied

                // Get the DbSeeder instance and call SeedData
                var dbSeeder = services.GetRequiredService<DbSeeder>();
                await dbSeeder.SeedData(); // Call the instance method
            }

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Configure default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
