using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace vibe_backend.Models
{
    public class Post
    {
        public int id { get; set; }

        public int userid { get; set; }
        public string? content { get; set; }
    }

    public class Feed
    {
        public int id { get; set; }
        public int userid { get; set; }
        public List<Post> posts { get; set; } = new();
    }

    public class FeedContext(DbContextOptions<FeedContext> options) : DbContext(options)
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Feed> Feeds { get; set; }
    }
}