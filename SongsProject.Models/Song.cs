using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SongsProject.Models
{
    public class Song
    {
        [Key]
        public int SongId { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string LyricsSummary { get; set; }
        public string AudioUrl { get; set; }
        public virtual ICollection<SongTag> SongTags { get; set; }
    }
}