using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProtectedDataController : ControllerBase
{
  [HttpGet("geral")]
  public IActionResult GetDadosGerais()
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    return Ok(new { Message = $"Dados gerais acessados pelo usuário: {User.Identity!.Name} (ID: {userId}" });
  }

  [HttpGet("admin")]
  [Authorize(Roles = "Admin")]
  public IActionResult GetDadosAdmin ()
  {
    return Ok(new { Message = "Dados de administrador acessados." });
  }

  [HttpGet("menager-ou-admin")]
  [Authorize(Roles = "Manager, Admin")]
  public IActionResult GetDadosManagerOuAdmin()
  {
    return Ok(new { Menager = "Dados de Manager ou Admin acessados." });
  }
}

