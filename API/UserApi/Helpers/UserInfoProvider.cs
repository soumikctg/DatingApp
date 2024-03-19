using System.Security.Claims;

namespace UserAPI.Helpers
{
    public class UserInfoProvider
    {
        private static int _cnt = 0;
        public static IHttpContextAccessor ContextAccessor { get; private set; }

        public static void SetContext(IHttpContextAccessor context)
        {
            if (context == null || _cnt > 0)
            {
                return;
            }
            _cnt++;
            ContextAccessor = context;
        }

        public static string CurrentUserName()
        {
            return ContextAccessor?.HttpContext?.User?.Identity?.Name;
        }

        public static List<Claim> GetClaims()
        {
            return ContextAccessor?.HttpContext?.User.Claims.ToList();
        }

        public static string GetCurrentUserId()
        {
            return GetClaims()?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
