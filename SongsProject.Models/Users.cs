using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SongsProject.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User"; // "Admin" או "User"
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<ListeningHistory> History { get; set; }
        public virtual ICollection<Recommendation> Recommendations { get; set; }
    }
}