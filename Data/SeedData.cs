using AppDev.Helpers;
using AppDev.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppDev.Data
{
    public class SeedData
    {

        public async static Task SeedAsync(IServiceProvider sp)
        {
            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                await roleManager.CreateAsync(new(Roles.User));
            }
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new(Roles.Admin));
            }
            if (!await roleManager.RoleExistsAsync(Roles.StoreOwner))
            {
                await roleManager.CreateAsync(new(Roles.StoreOwner));
            }

            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();

            if (await userManager.FindByNameAsync("admin@gmail.com") == null)
            {
                var admin = new ApplicationUser()
                {
                    UserName = "admin@gmail.com",
                    FullName = "Administrator",
                    HomeAddress = "",
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(admin, "Asd@123");

                await userManager.AddToRoleAsync(admin, "Admin");
            }

            var db = sp.GetRequiredService<ApplicationDbContext>();

            List<Category> categories = new()
            {
                new Category()
                {
                    Name = "Truyen Tranh"
                },
                new Category()
                {
                    Name = "Co tich",
                },
                new Category()
                {
                    Name= "Tieu thuyet"
                }
            };

            foreach (var category in categories)
            {
                if (!await db.Categories.AnyAsync(c => c.Name == category.Name))
                {
                    await db.Categories.AddAsync(category);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
