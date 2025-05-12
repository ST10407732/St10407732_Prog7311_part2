using AgriConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AgriConnect.Data
{
    public class AgriDbContext : IdentityDbContext<User>
    {
        public AgriDbContext(DbContextOptions<AgriDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<FarmerProfile> FarmerProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

      

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // Suppress or log pending model changes warning
            optionsBuilder.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
