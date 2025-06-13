using Microsoft.AspNetCore.Identity;
using ProdutosAPi.Models;

namespace ProdutosAPi.Data 
{
  public static class SeedData 
  {
    public static async Task Initialize(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) 
    {
      string[] roleNames = { "Admin", "User", "Menager" }; //Definindo as roles(categorias de usuarios) que a aplicacao vai ter
      IdentityResult roleResult; // Estou criando uma variavel que é feita para guardar informações do tipo IdentityResult

      foreach (var roleName in roleNames) {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist) {
          // criando a role e a adicionando ao banco de dados, caso não seja encontrado
          roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
      }
      
      var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");

      // Criando um novo usuário Admin padrao
      if (adminUser == null) {
        adminUser = new ApplicationUser {
          UserName = "admin",
          Email = "admin@gmail.com",
          EmailConfirmed = true // Confirmando o email diratamente para o admin
        };

        var createAdminUserResult = await userManager.CreateAsync(adminUser, "Senha123!");
        
        if (createAdminUserResult.Succeeded) {
          // Adiciona o usuário Admin a role "admin"
          await userManager.AddToRoleAsync(adminUser, "Admin");
        }
      }

    }
  }
}
