using System.ComponentModel.DataAnnotations;

namespace ProdutosAPi.DTOs
{
  public class RegisterDto
  {
    public required string Username { get; set; }

    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    public required string Email { get; set; }

    [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
    public required string Password { get; set; }

  }
}
