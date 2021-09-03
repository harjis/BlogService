using System;
using Microsoft.EntityFrameworkCore;

namespace PostService.DbMigrations
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running migrations");
            var dbContext = new PostDbContextFactory().CreateDbContext(args);
            
            dbContext.Database.Migrate();
            
            Console.WriteLine("Migrations have been run");
        }
    }
}
