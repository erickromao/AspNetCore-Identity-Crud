using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProdutosAPi.Models;
using ProdutosAPi.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly IConfiguration _configuration;

  public AuthController(
      UserManager<ApplicationUser> userManager,
      RoleManager<IdentityRole> roleManager,
      IConfiguration configuration
      ) 
  {
    _userManager = userManager;
    _roleManager = roleManager;
    _configuration = configuration;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterDto model)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var userExists = await _userManager.FindByNameAsync(model.Username);
    if (userExists != null)
    {
      return StatusCode(StatusCodes.Status409Conflict, new { Message = "Nome do usuário já existe!"});
    }
    
    var emailExists = await _userManager.FindByEmailAsync(model.Email);
    if  (emailExists != null)
    {
        return StatusCode(StatusCodes.Status409Conflict, new { Message = "E-mail já cadastrado!"});
    }

    return Ok(new { Message = "Usuário registrado com sucesso!"});
  }
}

