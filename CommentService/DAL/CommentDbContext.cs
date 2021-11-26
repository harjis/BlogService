using CommentService.Integration.Models;
using CommentService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentService.DAL
{
    public class CommentDbContext : DbContext
    {
        public CommentDbContext(DbContextOptions<CommentDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Post>().ToTable("Posts");
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Outbox.Consumer.Models.ConsumedEvent> ConsumedEvents { get; set; }
    }
}