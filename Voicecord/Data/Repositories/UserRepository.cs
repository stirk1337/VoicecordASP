using Voicecord.Interfaces;
using Voicecord.Models;

namespace Voicecord.Data.Repositories
{
    public class UserRepository : IBaseRepository<ApplicationUser>
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<ApplicationUser> GetAll()
        {
            return _db.Users;
        }

        public async Task Delete(ApplicationUser entity)
        {
            _db.Users.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(ApplicationUser entity)
        {
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<ApplicationUser> Update(ApplicationUser entity)
        {
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
