using IdentityApi.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApi.Data
{
    public class SeedData
    {
         public static async Task Initialize(ApplicationContext context,
                              UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            string password = "123456";

            string role1 = "Admin";
            string role2 = "Staff";

            if (await roleManager.FindByNameAsync(role1) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role1));
            }
            if (await roleManager.FindByNameAsync(role2) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role2));
            }

            if (await userManager.FindByNameAsync("0913111111") == null)
            {
                var user = new User
                {
                    Fullname = "Admin 1",
                    UserName = "0913111111",
                    Email = "admin1@gmail.com",
                    PhoneNumber = "0913111111"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
            }

            if (await userManager.FindByNameAsync("0913222222") == null)
            {
                var user = new User
                {
                    Fullname = "Admin 2",
                    UserName = "0913222222",
                    Email = "admin2@gmail.com",
                    PhoneNumber = "0913222222"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role1);
                }
            }

            if (await userManager.FindByNameAsync("0913333333") == null)
            {
                var user = new User
                {
                    Fullname = "Staff 1",
                    UserName = "0913333333",
                    Email = "staff1@gmail.com",
                    PhoneNumber = "0913333333"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            if (await userManager.FindByNameAsync("0913444444") == null)
            {
                var user = new User
                {
                    Fullname = "Staff 2",
                    UserName = "0913444444",
                    Email = "staff2@gmail.com",
                    PhoneNumber = "0913444444"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, role2);
                }
            }

            if (await userManager.FindByNameAsync("0913555555") == null)
            {
                var user = new User
                {
                    Fullname = "Customer 1",
                    UserName = "0913555555",
                    Email = "customer1@gmail.com",
                    PhoneNumber = "0913555555"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                }
            }

            if (await userManager.FindByNameAsync("0913666666") == null)
            {
                var user = new User
                {
                    Fullname = "Customer 2",
                    UserName = "0913666666",
                    Email = "customer2@gmail.com",
                    PhoneNumber = "0913666666"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                }
            }
        }
    }
}