using System;
using Microsoft.EntityFrameworkCore;

namespace CommentService.DbMigrations
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running migrations");
            var dbContext = new CommentDbContextFactory().CreateDbContext(args);
            
            dbContext.Database.Migrate();
            
            Console.WriteLine("Migrations have been run");
        }
    }
}
