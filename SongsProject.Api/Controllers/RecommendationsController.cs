using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using SongsProject.Repositories.Interfaces;
using SongsProject.Services;

namespace SongsProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : ControllerBase
    {
        private readonly RecommendationEngineService _engine;
        private readonly ISongRepository _songRepo;
        private readonly IRecommendationRepository _recommendationRepo;

        public RecommendationsController(
            RecommendationEngineService engine,
            ISongRepository songRepo,
            IRecommendationRepository recommendationRepo)
        {
            _engine = engine;
            _songRepo = songRepo;
            _recommendationRepo = recommendationRepo;
        }

        // AI recommendations
        [HttpGet("generate/{userId}")]
        public async Task<IActionResult> Generate(int userId)
        {
            var songs = await _engine.GenerateRecommendations(userId);
            return Ok(songs);
        }

        // Time based
        [HttpGet("now/{userId}")]
        public async Task<IActionResult> GetNow(int userId)
        {
            var songs = await _engine.GetTimeBasedSongs(userId);
            return Ok(songs);
        }

        // History recommendations (saved)
        [HttpGet("saved/{userId}")]
        public IActionResult GetSaved(int userId)
        {
            var recommendations = _recommendationRepo.GetAll()
                .Where(r => r.UserId == userId)
                .ToList();

            var songIds = recommendations.Select(r => r.SongId).ToList();
            var songs = _songRepo.GetAll()
                .Where(s => songIds.Contains(s.SongId))
                .ToList();

            return Ok(songs);
        }
    }
}