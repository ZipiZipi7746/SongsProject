using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using SongsProject.Services;
using SongsProject.Services.Interfaces;

namespace SongsProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {

        private readonly ISongService _service;
        private readonly RecommendationEngineService _recommendationEngine;

        public SongsController(ISongService service, RecommendationEngineService recommendationEngine)
        {
            _service = service;
            _recommendationEngine = recommendationEngine;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var song = await _service.GetByIdAsync(id);
            if (song == null) return NotFound();
            return Ok(song);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Song song) => Ok(await _service.AddAsync(song));

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Dictionary<string, object> fields)
        {
            var result = await _service.UpdateAsync(id, fields);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadSong(IFormFile audioFile, [FromForm] string artistName)
        {
            if (audioFile == null || audioFile.Length == 0)
                return BadRequest("לא הועלה קובץ");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audio");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(audioFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await audioFile.CopyToAsync(stream);
            }

            var audioUrl = $"http://localhost:5219/audio/{fileName}";

            var aiService = new AIService();
            var result = await aiService.AnalyzeSongMetadata(filePath);

            var song = new Song
            {
                Title = result.Title,
                ArtistName = artistName,
                Genre = result.Genre,
                LyricsSummary = result.Summary,
                AudioUrl = audioUrl,
                ReleaseDate = DateTime.Now
            };

            var dto = await _service.AddAsync(song);

            // AI בודק לכל לקוח אם השיר מתאים
            _ = Task.Run(async () =>
            {
                await _recommendationEngine.UpdateRecommendationsForAllUsers(dto.SongId);
            });

            return Ok(dto);
        }
    }
}