using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppCore.Entities;

namespace TodoAppCore.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "admin"},
                new AppRole{Name = "user"}
            };

            if (!await roleManager.Roles.AnyAsync())
            {
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            var users = new List<AppUser> {
                new AppUser { UserName = "hoainam10th" },
                new AppUser{ UserName="ubuntu" },
                new AppUser{ UserName="lisa" }
            };

            foreach (var user in users)
            {
                //user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "123456");
                await userManager.AddToRoleAsync(user, "user");
            }

            var admin = new AppUser { UserName = "admin" };
            await userManager.CreateAsync(admin, "123456");
            await userManager.AddToRolesAsync(admin, new[] { "admin", "user" });
        }
    }
}
