using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommentService.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommentService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString;
            if (Configuration.GetValue<string>("MSSQL-HOST") != null)
            {
                var dbHost = Configuration.GetValue<string>("MSSQL-HOST");
                var dbName = Configuration.GetValue<string>("DATABASE_DEVELOPMENT");
                var dbUser = Configuration.GetValue<string>("MSSQl-USER");
                var dbPassword = Configuration.GetValue<string>("MSSQl-PASSWORD");
                connectionString = $"Data Source={dbHost};Database={dbName};User Id={dbUser};Password={dbPassword};";
            }
            else
            {
                connectionString =
                    "Data Source=localhost;Database=comment-service-db;User Id=sa;Password=verystrongPassword123;";
            }

            services.AddDbContext<CommentDbContext>(options => options.UseSqlServer(connectionString));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CommentDbContext context)
        {
            // I think this needs to be before all routing related middleware (UseStaticFiles, UseRouting etc.)
            app.UsePathBase("/comments");

            // TODO Running migrations on application startup is not what I want but that is the best
            // I can do for now.
            if (context.Database.GetPendingMigrations().Any())
            {
                Console.WriteLine("Applying migrations...");
                context.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Comments}/{action=Index}/{id?}");
            });
        }
    }
}