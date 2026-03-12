using SongsProject.Models;
using SongsProject.Models.DTOs;
using SongsProject.Repositories.Interfaces;
using SongsProject.Services.Interfaces;

namespace SongsProject.Services
{
    public class RecommendationServiceImpl : IRecommendationService
    {
        private readonly IRecommendationRepository _repo;

        public RecommendationServiceImpl(IRecommendationRepository repo) { _repo = repo; }

        public IEnumerable<RecommendationDto> GetAll() => _repo.GetAll().ToList().Select(Mapper.ToDto);

        public async Task<RecommendationDto?> GetByIdAsync(int id)
        {
            var rec = await _repo.GetByIdAsync(id);
            return rec == null ? null : Mapper.ToDto(rec);
        }

        public async Task<RecommendationDto> AddAsync(Recommendation rec)
        {
            await _repo.AddAsync(rec);
            return Mapper.ToDto(rec);
        }

        public async Task<RecommendationDto?> UpdateAsync(int id, Dictionary<string, object> fields)
        {
            var rec = await _repo.GetByIdAsync(id);
            if (rec == null) return null;
            if (fields.ContainsKey("score")) rec.Score = double.Parse(fields["score"].ToString()!);
            if (fields.ContainsKey("isSeen")) rec.IsSeen = bool.Parse(fields["isSeen"].ToString()!);
            await _repo.UpdateAsync(rec);
            return Mapper.ToDto(rec);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rec = await _repo.GetByIdAsync(id);
            if (rec == null) return false;
            await _repo.DeleteAsync(rec);
            return true;
        }
    }
}