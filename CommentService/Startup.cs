using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommentService.BackgroundServices;
using CommentService.DAL;
using CommentService.Integration;
using CommentService.Integration.Dto;
using CommentService.Integration.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Outbox.Consumer.Repositories;

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
            if (Configuration.GetValue<string>("POSTGRES_HOST") != null)
            {
                var dbHost = Configuration.GetValue<string>("POSTGRES_HOST");
                var dbName = Configuration.GetValue<string>("POSTGRES_DATABASE");
                var dbUser = Configuration.GetValue<string>("POSTGRES_USERNAME");
                var dbPassword = Configuration.GetValue<string>("POSTGRES_PASSWORD");
                connectionString = $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPassword};";
            }
            else
            {
                connectionString = $"Host=localhost;Database=comment-service-db;Username=postgres;Password=postgres;";
            }

            services.AddDbContext<CommentDbContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<CommentsRepository>();
            services.AddScoped<PostRepository>();
            services.AddScoped<ConsumedEventRepository<CommentDbContext, Post>>();
            services.AddScoped<PostsConsumer>();
            services.AddHostedService<PostsBackgroundService>();
            
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CommentDbContext context)
        {
            // I think this needs to be before all routing related middleware (UseStaticFiles, UseRouting etc.)
            app.UsePathBase("/comments");

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