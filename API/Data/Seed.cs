using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using API.DTOs;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IUserRepository userRepository)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            /*            int idCounter = 0;
                        foreach (var user in users)
                        {
                            user.UserName = user.UserName.ToLower();
                            user.Id = idCounter;
                            await userManager.CreateAsync(user, "Pa$$w0rd");
                            await userRepository.AddUserAsync(user);
                            idCounter++;
                        }
            */
            var roles = new List<AppRole>
            {
                new AppRole { Name = "Member" },
                new AppRole { Name = "Admin" },
                new AppRole { Name = "Moderator" },
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
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
