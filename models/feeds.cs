using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace vibe_backend.models
{
    public class Feed
    {
        [Key]
        public int feed_id { get; set; }
        public int userid { get; set; }
        //public List<Post> posts { get; set; } = new();
    }
}
