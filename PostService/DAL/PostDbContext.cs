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
        public DbSet<Outbox.Producer.Models.Outbox> Outboxes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Outbox.Producer.Models.Outbox>().ToTable("Outbox");
        }
    }
}