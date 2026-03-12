using SongsProject.Models;
using SongsProject.Models.DTOs;
using SongsProject.Repositories.Interfaces;
using SongsProject.Services.Interfaces;

namespace SongsProject.Services
{
    public class SongServiceImpl : ISongService
    {
        private readonly ISongRepository _repo;

        public SongServiceImpl(ISongRepository repo) { _repo = repo; }

        public IEnumerable<SongDto> GetAll() => _repo.GetAll().ToList().Select(Mapper.ToDto);

        public async Task<SongDto?> GetByIdAsync(int id)
        {
            var song = await _repo.GetByIdAsync(id);
            return song == null ? null : Mapper.ToDto(song);
        }

        public async Task<SongDto> AddAsync(Song song)
        {
            await _repo.AddSongAsync(song);
            return Mapper.ToDto(song);
        }

        public async Task<SongDto?> UpdateAsync(int id, Dictionary<string, object> fields)
        {
            var song = await _repo.GetByIdAsync(id);
            if (song == null) return null;
            if (fields.ContainsKey("title")) song.Title = fields["title"].ToString()!;
            if (fields.ContainsKey("artistName")) song.ArtistName = fields["artistName"].ToString()!;
            if (fields.ContainsKey("genre")) song.Genre = fields["genre"].ToString()!;
            if (fields.ContainsKey("lyricsSummary")) song.LyricsSummary = fields["lyricsSummary"].ToString()!;
            await _repo.UpdateAsync(song);
            return Mapper.ToDto(song);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var song = await _repo.GetByIdAsync(id);
            if (song == null) return false;
            await _repo.DeleteAsync(song);
            return true;
        }

        public async Task<SongDto> UploadSongAsync(Stream fileStream, string fileName, string artistName)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), fileName);
            using (var file = new FileStream(tempPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(file);
            }

            var aiService = new AIService();
            var result = await aiService.AnalyzeSongMetadata(tempPath);

            var song = new Song
            {
                Title = result.Title,
                ArtistName = artistName,
                Genre = result.Genre,
                LyricsSummary = result.Summary,
                ReleaseDate = DateTime.Now
            };

            await _repo.AddSongAsync(song);
            File.Delete(tempPath);
            return Mapper.ToDto(song);
        }
    }
}