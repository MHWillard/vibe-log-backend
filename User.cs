using Microsoft.EntityFrameworkCore;

namespace vibe_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    class UserDb : DbContext
    {
        public UserDb(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; } = null!;
    }
}
