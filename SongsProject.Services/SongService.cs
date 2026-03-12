using SongsProject.Models;
using SongsProject.Repositories.Interfaces;

namespace SongsProject.Services
{
    public class SongService
    {
        private readonly ISongRepository _repo;
        private AIService _aiService = new AIService();

        public SongService(ISongRepository repo) { _repo = repo; }

        public async Task<Song> ProcessAndSaveSong(string filePath)
        {
            var aiResult = await _aiService.AnalyzeSongMetadata(filePath);

            var newSong = new Song
            {
                Title = aiResult.Title,
                ArtistName = aiResult.Artist,
                Genre = aiResult.Genre,
                ReleaseDate = DateTime.Now
            };

            await _repo.AddSongAsync(newSong);
            return newSong;
        }
    }
}