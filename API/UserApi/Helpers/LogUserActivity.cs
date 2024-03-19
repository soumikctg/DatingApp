using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using UserAPI.Interfaces;

namespace UserAPI.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            var userRepository = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user = await userRepository.GetUserByIdAsync(int.Parse(userId));
            user.LastActive = DateTime.UtcNow;
            await uow.SaveChangesAsync();
        }
    }
}
