using Microsoft.AspNetCore.Mvc;
using UserAPI.DTOs;
using UserAPI.Interfaces;

namespace UserAPI.Controllers;

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