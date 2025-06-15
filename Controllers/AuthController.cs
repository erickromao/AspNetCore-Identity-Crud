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

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginDto model)
  {
    if (!ModelState.IsValid) 
    {
      return BadRequest(ModelState);
    }

    ApplicationUser? user = null;
    if (model.UsernameOrEmail.Contains("@"))
    {
      user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
    }
    else
    {
      user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
    }

    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
    {
      var userRoles = await _userManager.GetRolesAsync(user);
      var authClaims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email!),
      };

      foreach (var userRole in userRoles)
      {
        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
      }

      var jwtSecret = _configuration["JWT_KEY"];
      var issuer = _configuration["JWT_ISSUER"];
      var audience = _configuration["JWT_AUDIENCE"];

      if (string.IsNullOrEmpty(jwtSecret))
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new {Message = "Erro de configuração interna do servidor."});
      }
      
      var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

      var token = new JwtSecurityToken (
          issuer: issuer,
          audience: audience,
          expires: DateTime.Now.AddHours(3),
          claims: authClaims,
          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
          );

      return Ok( new
      {
        token = new JwtSecurityTokenHandler().WriteToken(token),
        expiration = token.ValidTo,
        username = user.UserName,
        email = user.Email,
        roles = userRoles
      });
    }

    return Unauthorized(new {Message = "Login ou senha inválidos."});
  }
}

