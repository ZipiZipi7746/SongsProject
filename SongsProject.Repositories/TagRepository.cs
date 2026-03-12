using SongsProject.Data;
using SongsProject.Models;
using SongsProject.Repositories.Interfaces;

namespace SongsProject.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly SongsDbContext db;
        public TagRepository(SongsDbContext context) { db = context; }

        public IQueryable<Tag> GetAll() => db.Tags;
        public async Task<Tag?> GetByIdAsync(int id) => await db.Tags.FindAsync(id);
        public async Task AddAsync(Tag tag) { db.Tags.Add(tag); await db.SaveChangesAsync(); }
        public async Task UpdateAsync(Tag tag) { db.Tags.Update(tag); await db.SaveChangesAsync(); }
        public async Task DeleteAsync(Tag tag) { db.Tags.Remove(tag); await db.SaveChangesAsync(); }
    }
}