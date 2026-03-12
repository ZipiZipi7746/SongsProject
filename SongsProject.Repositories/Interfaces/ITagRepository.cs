using SongsProject.Models;

namespace SongsProject.Repositories.Interfaces
{
    public interface ITagRepository
    {
        IQueryable<Tag> GetAll();
        Task<Tag?> GetByIdAsync(int id);
        Task AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(Tag tag);
    }
}