using SongsProject.Data;
using SongsProject.Models;
using SongsProject.Repositories.Interfaces;

namespace SongsProject.Repositories
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly SongsDbContext db;
        public RecommendationRepository(SongsDbContext context) { db = context; }

        public IQueryable<Recommendation> GetAll() => db.Recommendations;
        public async Task<Recommendation?> GetByIdAsync(int id) => await db.Recommendations.FindAsync(id);
        public async Task AddAsync(Recommendation rec) { db.Recommendations.Add(rec); await db.SaveChangesAsync(); }
        public async Task UpdateAsync(Recommendation rec) { db.Recommendations.Update(rec); await db.SaveChangesAsync(); }
        public async Task DeleteAsync(Recommendation rec) { db.Recommendations.Remove(rec); await db.SaveChangesAsync(); }
    }
}