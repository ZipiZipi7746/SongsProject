using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SongsProject.Models
{
    public class ListeningHistory
    {
        [Key]
        public int HistoryId { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        public int SongId { get; set; }
        [ForeignKey("SongId")]
        public virtual Song Song { get; set; }

        public DateTime ListenDate { get; set; }
        public int Duration { get; set; }
    }
}