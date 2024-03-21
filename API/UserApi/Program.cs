using DatingApp.Shared.Configurations;
using UserAPI.Extensions;
using UserAPI.Helpers;
using UserAPI.Middleware;
using UserAPI.SignalR;

var builder = WebApplication.CreateBuilder(args);

#region ServiceRegistration

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddMassTransit(typeof(Program).Assembly);

#endregion

var app = builder.Build();

UserInfoProvider.SetContext(app.Services.GetRequiredService<IHttpContextAccessor>());

#region StartupService

await app.DoMigrationsAsync();

#endregion

#region Middlewares    
// Configure the HTTP request pipeline.

app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("https://localhost:4200"));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

#endregion

app.Run();