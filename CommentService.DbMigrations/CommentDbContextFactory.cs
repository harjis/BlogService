using CommentService.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CommentService.DbMigrations
{
    public class CommentDbContextFactory : IDesignTimeDbContextFactory<CommentDbContext>
    {
        public CommentDbContextFactory()
        {
            
        }

        public CommentDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CommentDbContext>();
            builder.UseNpgsql(GetConnectionString(), optionsBuilder => optionsBuilder.MigrationsAssembly("CommentService.DbMigrations"));
            
            return new CommentDbContext(builder.Options);
        }
        
        private string GetConnectionString()
        {
            var envVariables = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            string connectionString;
            if (envVariables.GetValue<string>("POSTGRES_HOST") != null)
            {
                var dbHost = envVariables.GetValue<string>("POSTGRES_HOST");
                var dbName = envVariables.GetValue<string>("POSTGRES_DATABASE");
                var dbUser = envVariables.GetValue<string>("POSTGRES_USERNAME");
                var dbPassword = envVariables.GetValue<string>("POSTGRES_PASSWORD");
                connectionString = $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPassword};";
            }
            else
            {
                connectionString = $"Host=localhost;Database=comment-service-db;Username=postgres;Password=postgres;";
            }

            return connectionString;
        }
    }
}
