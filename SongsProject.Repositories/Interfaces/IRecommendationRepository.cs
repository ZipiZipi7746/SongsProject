using SongsProject.Models;

namespace SongsProject.Repositories.Interfaces
{
    public interface IRecommendationRepository
    {
        IQueryable<Recommendation> GetAll();
        Task<Recommendation?> GetByIdAsync(int id);
        Task AddAsync(Recommendation recommendation);
        Task UpdateAsync(Recommendation recommendation);
        Task DeleteAsync(Recommendation recommendation);
    }
}