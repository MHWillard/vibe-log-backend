using System.ComponentModel.DataAnnotations;

namespace vibe_backend.models 
{
    public class Post
    {
        [Key]
        public int post_id { get; set; }
        public int userid { get; set; }
        public string content { get; set; }
    }
}