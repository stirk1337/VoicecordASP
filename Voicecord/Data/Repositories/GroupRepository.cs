using Voicecord.Interfaces;
using Voicecord.Models;

namespace Voicecord.Data.Repositories
{
    public class GroupRepository : IBaseRepository<UserGroup>
    {
        private readonly ApplicationDbContext _db;

        public GroupRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<UserGroup> GetAll()
        {
            return _db.Groups;
        }

        public async Task Delete(UserGroup entity)
        {
            _db.Groups.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(UserGroup entity)
        {
            await _db.Groups.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<UserGroup> Update(UserGroup entity)
        {
            _db.Groups.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
