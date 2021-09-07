using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PostService.DAL;

namespace PostService.DbMigrations
{
    public class PostDbContextFactory : IDesignTimeDbContextFactory<PostDbContext>
    {
        public PostDbContextFactory()
        {
            
        }

        public PostDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PostDbContext>();
            builder.UseNpgsql(GetConnectionString(), optionsBuilder => optionsBuilder.MigrationsAssembly("PostService.DbMigrations"));
            
            return new PostDbContext(builder.Options);
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
                connectionString = $"Host=localhost;Database=post-service-db;Username=postgres;Password=postgres;";
            }

            return connectionString;
        }
    }
}
