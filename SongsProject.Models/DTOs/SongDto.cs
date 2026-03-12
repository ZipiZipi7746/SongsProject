namespace SongsProject.Models.DTOs
{
    public class SongDto
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string LyricsSummary { get; set; }
        public string AudioUrl { get; set; }
    }
}