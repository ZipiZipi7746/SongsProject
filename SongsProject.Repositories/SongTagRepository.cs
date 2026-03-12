using Microsoft.EntityFrameworkCore;
using SongsProject.Data;
using SongsProject.Models;
using SongsProject.Repositories.Interfaces;

namespace SongsProject.Repositories
{
    public class SongTagRepository : ISongTagRepository
    {
        private readonly SongsDbContext db;
        public SongTagRepository(SongsDbContext context) { db = context; }

        public IQueryable<SongTag> GetAll() => db.SongTags;
        public async Task<SongTag?> GetByIdAsync(int songId, int tagId) =>
            await db.SongTags.FirstOrDefaultAsync(st => st.SongId == songId && st.TagId == tagId);
        public async Task AddAsync(SongTag songTag) { db.SongTags.Add(songTag); await db.SaveChangesAsync(); }
        public async Task DeleteAsync(SongTag songTag) { db.SongTags.Remove(songTag); await db.SaveChangesAsync(); }
    }
}