using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Helpers;
using UserAPI.Interfaces;
using UserAPI.Services;
using UserAPI.SignalR;

namespace UserAPI.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAccountsService, AccountsService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddSingleton<IGlobalCache, GlobalCache>();



        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        services.AddScoped<LogUserActivity>();

        services.AddSignalR();
        services.AddSingleton<PresenceTracker>();


        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}