using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IGlobalCache _globalCache;

        public HomeController(IGlobalCache globalCache)
        {
            _globalCache = globalCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetHomeData()
        {
            return Ok(_globalCache.GetValue<HomeDto>("HomeData"));
        }
    }
}
