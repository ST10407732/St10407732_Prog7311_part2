using AgriConnect.Models;
using AgriConnect.Data;
using Microsoft.AspNetCore.Identity;
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

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ✅ Call SeedData *after* building the app
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await DbSeeder.SeedData(services); // 👈 Use your DbSeeder here
            }

            // Pipeline
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
