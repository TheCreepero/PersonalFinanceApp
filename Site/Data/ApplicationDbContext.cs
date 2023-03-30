using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Site.Models;

namespace Site.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account>? Account { get; set; }
        public DbSet<Site.Models.Transaction>? Transaction { get; set; }
        public DbSet<Site.Models.Month>? Month { get; set; }
    }
}