using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using vibe_backend.models;

namespace vibe_backend.models
{
    public class FeedContext : DbContext {
        public FeedContext(DbContextOptions<FeedContext> options) : base(options)
        {
        }

        public DbSet<Post> posts { get; set; }
        public DbSet<Feed> feeds { get; set; }
    }
}