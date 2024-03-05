﻿using API.Data;
using API.Factories;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddSingleton<IGlobalCache, GlobalCache>();
            services.AddScoped<ILikesRepository, LikeRepository>();
            services.AddScoped<IAccountsRepository, SqlAccountsRepository>();
            services.AddScoped<IMessageRepository, MongoMessageRepository>();
            services.AddScoped<IMessageRepository, SqlMessageRepository>();

            services.AddScoped<IMessageRepositoryFactory, MessageRepositoryFactory>();


            return services;
        }
    }
}
