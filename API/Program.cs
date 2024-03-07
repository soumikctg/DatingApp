using API.Extensions;
using API.Middleware;
using API.SignalR;

var builder = WebApplication.CreateBuilder(args);

#region ServiceRegistration

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

#endregion

var app = builder.Build();

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