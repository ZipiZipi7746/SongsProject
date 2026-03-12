using SongsProject.Models;

namespace SongsProject.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<Users> GetAll();
        Task<Users?> GetByIdAsync(int id);
        Task AddAsync(Users user);
        Task UpdateAsync(Users user);
        Task DeleteAsync(Users user);
    }
}