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

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Seed the database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ZooContext>();
                    context.Database.Migrate(); // Apply any pending migrations

                    // Ensure the database is created and seeded
                    var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());
                    SeedData.Seed(modelBuilder);

                    foreach (var entity in modelBuilder.Model.GetEntityTypes())
                    {
                        var data = modelBuilder.Model.FindEntityType(entity.Name).GetSeedData();
                        if (data.Any())
                        {
                            context.AddRange(data);
                        }
                    }

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

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
