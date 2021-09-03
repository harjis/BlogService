using Microsoft.EntityFrameworkCore;
using PostService.Models;

namespace PostService.DAL
{
    public class PostDbContext : DbContext
    {
        public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Outbox.Outbox.Models.Outbox> Outboxes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Outbox.Outbox.Models.Outbox>().ToTable("Outbox");
        }
    }
}