using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SongsProject.Models
{
    public class Recommendation
    {
        [Key]
        public int RecommendationId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
        public int SongId { get; set; }
        [ForeignKey("SongId")]
        public virtual Song Song { get; set; }
        public double Score { get; set; }
        public bool IsSeen { get; set; }
        public DateTime RecommendedAt { get; set; } = DateTime.Now;
    }
}