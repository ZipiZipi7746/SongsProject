using SongsProject.Models;

namespace SongsProject.Repositories.Interfaces
{
    public interface ISongRepository
    {
        IQueryable<Song> GetAll();
        Task<Song?> GetByIdAsync(int id);
        Task AddSongAsync(Song song);
        Task DeleteAsync(Song song);
        Task UpdateAsync(Song song);
    }
}