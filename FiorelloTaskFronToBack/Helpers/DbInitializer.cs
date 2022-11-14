using FiorelloTaskFronToBack.Constants;
using FiorelloTaskFronToBack.Models;
using Microsoft.AspNetCore.Identity;

namespace FiorelloTaskFronToBack.Helpers
{
    public static class DbInitializer
    {

        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    });
                }
            }

            if (await userManager.FindByNameAsync("admin") == null)
            {
                var user = new User
                {
                    UserName = "admin",
                    Fullname = "Admin Adminov",
                    Email = "admin@gmail.com"
                };
                var result = await userManager.CreateAsync(user, "Admin1234*");
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        throw new Exception(error.Description.ToString());
                    }
                }
                await userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
            }
        }
    }
}
