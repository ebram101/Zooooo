using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Zoo.Data;
using Zoo.Models;

namespace Zoo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure the database context
            builder.Services.AddDbContext<ZooContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("ZooContext") ?? throw new InvalidOperationException("Connection string 'ZooContext' not found.")));
            builder.Services.AddScoped<ZooSeeder>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Seed the database
            var serviceProvider = app.Services.CreateScope().ServiceProvider;
            var seeder = serviceProvider.GetRequiredService<ZooSeeder>();
            seeder.DataSeeder();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
