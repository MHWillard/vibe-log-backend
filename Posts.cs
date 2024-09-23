using Microsoft.EntityFrameworkCore;



namespace vibe_backend.Models
{
    public class Post
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string? Content { get; set; }
    }

    public class Feed
    {
        public int UserId { get; set; }
        public List<Post> Posts { get; set; }
    }

    class PostDb : DbContext
    {
        public PostDb(DbContextOptions options) : base(options) { }
        public DbSet<Post> Posts { get; set; } = null!;
    }
}