using SongsProject.Models;
using SongsProject.Models.DTOs;

namespace SongsProject.Services.Interfaces
{
    public interface IRecommendationService
    {
        IEnumerable<RecommendationDto> GetAll();
        Task<RecommendationDto?> GetByIdAsync(int id);
        Task<RecommendationDto> AddAsync(Recommendation rec);
        Task<RecommendationDto?> UpdateAsync(int id, Dictionary<string, object> fields);
        Task<bool> DeleteAsync(int id);
    }
}