using SongsProject.Models;

namespace SongsProject.Repositories.Interfaces
{
    public interface IListeningHistoryRepository
    {
        IQueryable<ListeningHistory> GetAll();
        Task<ListeningHistory?> GetByIdAsync(int id);
        Task AddAsync(ListeningHistory history);
        Task DeleteAsync(ListeningHistory history);
    }
}