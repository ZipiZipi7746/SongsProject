using SongsProject.Data;
using SongsProject.Models;
using SongsProject.Repositories.Interfaces;

namespace SongsProject.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly SongsDbContext db;
        public SongRepository(SongsDbContext context) { db = context; }

        public IQueryable<Song> GetAll() => db.Songs;
        public async Task<Song?> GetByIdAsync(int id) => await db.Songs.FindAsync(id);
        public async Task AddSongAsync(Song song) { db.Songs.Add(song); await db.SaveChangesAsync(); }
        public async Task UpdateAsync(Song song) { db.Songs.Update(song); await db.SaveChangesAsync(); }
        public async Task DeleteAsync(Song song) { db.Songs.Remove(song); await db.SaveChangesAsync(); }
    }
}