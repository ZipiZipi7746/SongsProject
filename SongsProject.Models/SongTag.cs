using System.ComponentModel.DataAnnotations.Schema;

namespace SongsProject.Models
{
    public class SongTag
    {
        public int SongId { get; set; }
        [ForeignKey("SongId")]
        public virtual Song Song { get; set; }

        public int TagId { get; set; }
        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }
    }
}