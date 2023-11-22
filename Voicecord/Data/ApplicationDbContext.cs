using Microsoft.EntityFrameworkCore;
using Voicecord.Models;

namespace Voicecord.Data
{
   public class ApplicationDbContext : DbContext
   {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserGroup> Groups { get; set; }
        
    }
}
