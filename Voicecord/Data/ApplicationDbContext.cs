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
        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<UserGroup> Groups { get; set; }
        
    }
}
