using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Data
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManeger, RoleManager<IdentityRole> roleManeger)
        {
            await SeedRoles(roleManeger);
            await SeedUsers(userManeger);
        }
        private async static Task SeedUsers(UserManager<IdentityUser> userManeger)
        {
            if (await userManeger.FindByEmailAsync("admin@bookstore.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@bookstore.com"
                };
                var result = await userManeger.CreateAsync(user, "P@ssword1");
                if (result.Succeeded)
                {
                    await userManeger.AddToRoleAsync(user, "Administrator");
                }
            }


            if (await userManeger.FindByEmailAsync("customer@gmail.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Customer",
                    Email = "customer@gmail.com"
                };
                var result = await userManeger.CreateAsync(user, "P@ssword1");
                if (result.Succeeded)
                {
                    await userManeger.AddToRoleAsync(user, "Customer");
                }
            }

            if (await userManeger.FindByEmailAsync("customer1@gmail.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Customer1",
                    Email = "customer1@gmail.com"
                };
                var result = await userManeger.CreateAsync(user, "P@ssword1");
                if (result.Succeeded)
                {
                    await userManeger.AddToRoleAsync(user, "Customer");
                }
            }
        }
        private async static Task SeedRoles(RoleManager<IdentityRole> roleManeger)
        {
            if (!await roleManeger.RoleExistsAsync("Administrator"))
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                await roleManeger.CreateAsync(role);
            }

            if (!await roleManeger.RoleExistsAsync("Customer"))
            {
                var role = new IdentityRole
                {
                    Name = "Customer"
                };
                await roleManeger.CreateAsync(role);
            }
        }
    }
}
