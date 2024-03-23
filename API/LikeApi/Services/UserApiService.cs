using System.Net.Http.Headers;
using System.Text.Json;
using DatingApp.Shared.Dtos;
using LikeApi.Interfaces;

namespace LikeApi.Services;

public class UserApiService : IUserApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<MemberDto> GetUserApiResponseAsync(string username)
    {
        var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new Exception("AccessToken not found.");
        }

        var client = _httpClientFactory.CreateClient();

        string token = accessToken.ToString();
        string[] accToken = token.Split(' ');

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accToken[1]);

        using var response = await client.GetAsync("http://localhost:5000/api/Users/" + username);
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<MemberDto>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
            return user;
        }
        else
        {
            throw new Exception($"Failed to retrieve data Status code: {response.StatusCode}");
        }
    }
}