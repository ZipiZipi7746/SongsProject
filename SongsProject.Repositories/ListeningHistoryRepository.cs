using SongsProject.Data;
using SongsProject.Models;
using SongsProject.Repositories.Interfaces;

namespace SongsProject.Repositories
{
    public class ListeningHistoryRepository : IListeningHistoryRepository
    {
        private readonly SongsDbContext db;
        public ListeningHistoryRepository(SongsDbContext context) { db = context; }

        public IQueryable<ListeningHistory> GetAll() => db.ListeningHistories;
        public async Task<ListeningHistory?> GetByIdAsync(int id) => await db.ListeningHistories.FindAsync(id);
        public async Task AddAsync(ListeningHistory history) { db.ListeningHistories.Add(history); await db.SaveChangesAsync(); }
        public async Task DeleteAsync(ListeningHistory history) { db.ListeningHistories.Remove(history); await db.SaveChangesAsync(); }
    }
}