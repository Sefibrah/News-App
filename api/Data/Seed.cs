using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Data {
    public class Seed {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<Role> roleManager) {
            if (await userManager.Users.AnyAsync()) return;

            var stream = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(stream);
            if (users == null) return;

            var roles = new List<Role> {
                new Role { Name = "Admin" },
                new Role { Name = "User" }
            };

            foreach (var r in roles) {
                await roleManager.CreateAsync(r);
            }

            foreach (var u in users) {
                await userManager.CreateAsync(u, "Pa$$w0rd");
                await userManager.AddToRoleAsync(u, "User");
            }

            var admin = new AppUser {
                UserName = "Admin",
                Email = "admin@gmail.com"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}