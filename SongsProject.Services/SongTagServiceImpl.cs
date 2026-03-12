using SongsProject.Models;
using SongsProject.Models.DTOs;
using SongsProject.Repositories.Interfaces;
using SongsProject.Services.Interfaces;

namespace SongsProject.Services
{
    public class SongTagServiceImpl : ISongTagService
    {
        private readonly ISongTagRepository _repo;

        public SongTagServiceImpl(ISongTagRepository repo) { _repo = repo; }

        public IEnumerable<SongTagDto> GetAll() => _repo.GetAll().ToList().Select(Mapper.ToDto);

        public async Task<SongTagDto> AddAsync(SongTag songTag)
        {
            await _repo.AddAsync(songTag);
            return Mapper.ToDto(songTag);
        }

        public async Task<bool> DeleteAsync(int songId, int tagId)
        {
            var songTag = await _repo.GetByIdAsync(songId, tagId);
            if (songTag == null) return false;
            await _repo.DeleteAsync(songTag);
            return true;
        }
    }
}