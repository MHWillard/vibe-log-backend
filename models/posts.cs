using System.ComponentModel.DataAnnotations;

namespace vibe_backend.models 
{
    public class Post
    {
        [Key]
        public int post_table_id {get; set;}
        public int user_id { get; set; }
        public string content { get; set; }

        public int post_id { get; set; }

        public DateTime post_date { get; set; }
    }
}