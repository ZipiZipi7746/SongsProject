namespace SongsProject.Models.DTOs
{
    public class ListeningHistoryDto
    {
        public int HistoryId { get; set; }
        public int UserId { get; set; }
        public int SongId { get; set; }
        public DateTime ListenDate { get; set; }
        public int Duration { get; set; }
    }
}