using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PostService.DbMigrations
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any() && args[0] == "drop=true")
            {
                Drop(args);
            }
            else
            {
                Migrate(args);
            }
        }

        private static void Migrate(string[] args)
        {
            Console.WriteLine("Running migrations");
            var dbContext = new PostDbContextFactory().CreateDbContext(args);

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }

            Console.WriteLine("Migrations have been run");
        }

        private static void Drop(string[] args)
        {
            Console.WriteLine("Dropping");
            
            var dbContext = new PostDbContextFactory().CreateDbContext(args);
            dbContext.Database.EnsureDeleted();

            Console.WriteLine("Drop finished");
        }
    }
}
