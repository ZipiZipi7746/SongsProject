using SongsProject.Models;

namespace SongsProject.Repositories.Interfaces
{
    public interface ISongTagRepository
    {
        IQueryable<SongTag> GetAll();
        Task<SongTag?> GetByIdAsync(int songId, int tagId);
        Task AddAsync(SongTag songTag);
        Task DeleteAsync(SongTag songTag);
    }
}