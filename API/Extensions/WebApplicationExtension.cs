using API.Data;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class WebApplicationExtension
    {
        public static async Task DoMigrationsAsync(this WebApplication app)
        {
            var enabled = app.Configuration.GetSection("MigrationEnabled").Get<bool>();

            if (!enabled) return;

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                var globalCache = services.GetRequiredService<IGlobalCache>();
                await context.Database.MigrateAsync();

                await Seed.SeedUsers(context);
                await Seed.SeedHomeData(globalCache);
            }
            catch (Exception ex)
            {
                var logger = services.GetService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");
            }
        }
    }
}
