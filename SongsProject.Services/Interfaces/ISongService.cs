using SongsProject.Models;
using SongsProject.Models.DTOs;

namespace SongsProject.Services.Interfaces
{
    public interface ISongService
    {
        IEnumerable<SongDto> GetAll();
        Task<SongDto?> GetByIdAsync(int id);
        Task<SongDto> AddAsync(Song song);
        Task<SongDto?> UpdateAsync(int id, Dictionary<string, object> fields);
        Task<bool> DeleteAsync(int id);
        Task<SongDto> UploadSongAsync(Stream fileStream, string fileName, string artistName);
    }
}