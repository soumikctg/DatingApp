using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.Services;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync())
            {
                Console.WriteLine("Skip for seeding user data");
                return;
            }

            Console.WriteLine("Start seeding user data");

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user);
            }
            await context.SaveChangesAsync();

            Console.WriteLine("Seeding user data success");
        }

        public static async Task SeedHomeData(IGlobalCache globalCache)
        {
            Console.WriteLine("Start seeding home data");

            var homeData = await File.ReadAllTextAsync("Data/HomeSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var homeDto = JsonSerializer.Deserialize<HomeDto>(homeData, options);

            globalCache.SetValue("HomeData", homeDto);

            Console.WriteLine("Seeding user data success");
        }
    }
}
