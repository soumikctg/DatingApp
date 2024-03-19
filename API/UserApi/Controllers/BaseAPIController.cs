using Microsoft.AspNetCore.Mvc;
using UserAPI.Helpers;

namespace UserAPI.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
    }
}
