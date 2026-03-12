using SongsProject.Models;
using SongsProject.Models.DTOs;
using SongsProject.Repositories.Interfaces;
using SongsProject.Services.Interfaces;

namespace SongsProject.Services
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _repo;

        public UserServiceImpl(IUserRepository repo) { _repo = repo; }

        public IEnumerable<UserDto> GetAll() => _repo.GetAll().ToList().Select(Mapper.ToDto);

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user == null ? null : Mapper.ToDto(user);
        }

        public async Task<UserDto> AddAsync(Users user)
        {
            await _repo.AddAsync(user);
            return Mapper.ToDto(user);
        }

        public async Task<UserDto?> UpdateAsync(int id, Dictionary<string, object> fields)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return null;
            if (fields.ContainsKey("username")) user.Username = fields["username"].ToString()!;
            if (fields.ContainsKey("email")) user.Email = fields["email"].ToString()!;
            await _repo.UpdateAsync(user);
            return Mapper.ToDto(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return false;
            await _repo.DeleteAsync(user);
            return true;
        }
    }
}