using System.ComponentModel.DataAnnotations;

namespace ProdutosAPi.DTOs 
{
  public class LoginDto
  {
    public required string UsernameOrEmail { get; set; }
  
    [MinLength(8, ErrorMessage = "Senha inválida")]
    public required string Password { get; set; }
  }
}
