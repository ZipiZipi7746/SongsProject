using SongsProject.Models;
using SongsProject.Models.DTOs;

namespace SongsProject.Services.Interfaces
{
    public interface ITagService
    {
        IEnumerable<TagDto> GetAll();
        Task<TagDto?> GetByIdAsync(int id);
        Task<TagDto> AddAsync(Tag tag);
        Task<TagDto?> UpdateAsync(int id, Dictionary<string, object> fields);
        Task<bool> DeleteAsync(int id);
    }
}