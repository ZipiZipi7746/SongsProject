using SongsProject.Models;
using SongsProject.Models.DTOs;

namespace SongsProject.Services.Interfaces
{
    public interface ISongTagService
    {
        IEnumerable<SongTagDto> GetAll();
        Task<SongTagDto> AddAsync(SongTag songTag);
        Task<bool> DeleteAsync(int songId, int tagId);
    }
}