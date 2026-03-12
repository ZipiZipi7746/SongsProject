using SongsProject.Data;
using SongsProject.Models;
using SongsProject.Repositories.Interfaces;

namespace SongsProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SongsDbContext db;
        public UserRepository(SongsDbContext context) { db = context; }

        public IQueryable<Users> GetAll() => db.Users;
        public async Task<Users?> GetByIdAsync(int id) => await db.Users.FindAsync(id);
        public async Task AddAsync(Users user) { db.Users.Add(user); await db.SaveChangesAsync(); }
        public async Task UpdateAsync(Users user) { db.Users.Update(user); await db.SaveChangesAsync(); }
        public async Task DeleteAsync(Users user) { db.Users.Remove(user); await db.SaveChangesAsync(); }
    }
}