﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Entities;
using UserAPI.Interfaces;

namespace UserAPI.Extensions;

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
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
            var userRepository = services.GetRequiredService<IUserRepository>();

            await context.Database.MigrateAsync();

            await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");

            await Seed.SeedUsers(userManager, roleManager, userRepository);
            await Seed.SeedHomeData(globalCache);
        }
        catch (Exception ex)
        {
            var logger = services.GetService<ILogger<Program>>();
            logger.LogError(ex, "An error occured during migration");
        }
    }
}