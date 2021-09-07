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
        }

        public DbSet<Comment> Comment { get; set; }
        public DbSet<Outbox.Consumer.Models.ConsumedEvent> ConsumedEvents { get; set; }
    }
}