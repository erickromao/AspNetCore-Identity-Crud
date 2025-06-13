using Microsoft.AspNetCore.Identity;
using ProdutosAPi.Models;

namespace ProdutosAPi.Data 
{
  public static class SeedData 
  {
    public static async Task Initialize(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) 
    {
      string[] roleNames = { "Admin", "User", "Menager" };
      IdentityResult roleResult;

      foreach (var roleName in roleNames)
      {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
          roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
      }
      
      var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");

      if (adminUser == null)
      {
        adminUser = new ApplicationUser
        {
          UserName = "admin",
          Email = "admin@gmail.com",
          EmailConfirmed = true 
        };

        var createAdminUserResult = await userManager.CreateAsync(adminUser, "Senha123!");
        
        if (createAdminUserResult.Succeeded)
        {
          await userManager.AddToRoleAsync(adminUser, "Admin");
        }
      }

    }
  }
}
