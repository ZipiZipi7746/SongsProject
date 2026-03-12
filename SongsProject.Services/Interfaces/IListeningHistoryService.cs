using SongsProject.Models;
using SongsProject.Models.DTOs;

namespace SongsProject.Services.Interfaces
{
    public interface IListeningHistoryService
    {
        IEnumerable<ListeningHistoryDto> GetAll();
        Task<ListeningHistoryDto?> GetByIdAsync(int id);
        Task<ListeningHistoryDto> AddAsync(ListeningHistory history);
        Task<bool> DeleteAsync(int id);
    }
}