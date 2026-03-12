using SongsProject.Models;
using SongsProject.Models.DTOs;

namespace SongsProject.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> AddAsync(Users user);
        Task<UserDto?> UpdateAsync(int id, Dictionary<string, object> fields);
        Task<bool> DeleteAsync(int id);
    }
}