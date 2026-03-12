using Microsoft.EntityFrameworkCore;
using SongsProject.Models;

namespace SongsProject.Data
{
    public class SongsDbContext : DbContext
    {
        public SongsDbContext(DbContextOptions<SongsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SongTag>().HasKey(st => new { st.SongId, st.TagId });
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<ListeningHistory> ListeningHistories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SongTag> SongTags { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
    }
}