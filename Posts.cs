using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
        public List<Post> Posts { get; set; } = new();
    }

    public class FeedContext: DbContext
    {
        public FeedContext(DbContextOptions<FeedContext> options) : base(options) { }
        //public PostDb(DbContextOptions options) : base(options) { }
        //public DbSet<Post> Posts { get; set; } = null!;

        public DbSet<Post> Posts { get; set;}
        public DbSet<Feed> Feeds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var connectionString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase";
            var connectionString = "postgresql://postgres:ShXfWkWPtUdVDSuvODIYXnUkXyOARfak@junction.proxy.rlwy.net:36689/railway";
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}