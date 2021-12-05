using HelloCMS.Identity.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelloCMS.Identity.Data
{
    public class AppDbContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, int>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {          
        }
        
        //Entities
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
