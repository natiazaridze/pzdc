using KartuliAPI1.Auth.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace KartuliAPI1.Data.Entities
{
    public class ForumDbContext : IdentityDbContext<ForumRestUser>
    {
        private readonly IConfiguration _configuration;
        public DbSet<Users> Topics { get; set; }
        public DbSet<Recipes> Posts { get; set; }
        public DbSet<Wines> Comments { get; set; }

        public ForumDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("PostgreSQL"));
        }
    }
}

