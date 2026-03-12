using SongsProject.Models;
using SongsProject.Models.DTOs;
using SongsProject.Repositories.Interfaces;
using SongsProject.Services.Interfaces;

namespace SongsProject.Services
{
    public class ListeningHistoryServiceImpl : IListeningHistoryService
    {
        private readonly IListeningHistoryRepository _repo;

        public ListeningHistoryServiceImpl(IListeningHistoryRepository repo) { _repo = repo; }

        public IEnumerable<ListeningHistoryDto> GetAll() => _repo.GetAll().ToList().Select(Mapper.ToDto);

        public async Task<ListeningHistoryDto?> GetByIdAsync(int id)
        {
            var history = await _repo.GetByIdAsync(id);
            return history == null ? null : Mapper.ToDto(history);
        }

        public async Task<ListeningHistoryDto> AddAsync(ListeningHistory history)
        {
            await _repo.AddAsync(history);
            return Mapper.ToDto(history);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var history = await _repo.GetByIdAsync(id);
            if (history == null) return false;
            await _repo.DeleteAsync(history);
            return true;
        }
    }
}