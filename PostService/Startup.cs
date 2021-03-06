using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Outbox.Producer.Managers;
using PostService.DAL;

namespace PostService
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
                connectionString = $"Host=localhost;Database=post-service-db;Username=postgres;Password=postgres;";
            }

            services.AddDbContext<PostDbContext>(options => options.UseNpgsql(connectionString));
            services.AddControllersWithViews();
            services.AddScoped<PostRepository>();
            services.AddScoped<EventManager<PostDbContext>>();
            services.AddScoped<Services.PostService>();
            services.AddScoped<UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PostDbContext context)
        {
            // I think this needs to be before all routing related middleware (UseStaticFiles, UseRouting etc.)
            app.UsePathBase("/posts");

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
                    pattern: "{controller=Posts}/{action=Index}/{id?}");
            });
        }
    }
}
