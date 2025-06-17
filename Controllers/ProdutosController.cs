using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosAPi.Data;
using ProdutosAPi.DTOs;
//using ProdutosAPi.Models;
//using System.Security.Claims;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
  private readonly ApplicationDbContext _context;
  private readonly IMapper _mapper;
  private readonly ILogger<ProdutosController> _logger;

  public ProdutosController(ApplicationDbContext context, IMapper mapper, ILogger<ProdutosController> logger)
  {
    _context = context;
    _mapper = mapper;
    _logger = logger;
  }

  // Obtendo todos produtos
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos()
  { 
    _logger.LogInformation("Listando todos os produtos.");

    var produtos = await _context.Produtos!.AsNoTracking().ToListAsync();

    var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);

    return Ok(produtosDto);
  }

  // Obtendo produto por ID
  [HttpGet("{id}")]
  public async Task<ActionResult<ProdutoDto>> GetProdutosPorId(int id)
  {
    _logger.LogInformation("Buscando produto com ID: {ProdutoId}", id);
    
    var produto = await _context.Produtos!.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    if (produto == null)
    {
      _logger.LogWarning("Produto com ID: {ProdutoId} não encontrado.", id);
      return NotFound(new { message = $"Produto com ID {id} não encontrado"});
    }

    var produtoDto = _mapper.Map<ProdutoDto>(produto);

    return Ok(produtoDto);
  }

  // Criar um produto
  [HttpPost]
  public async Task<ActionResult<ProdutoDto>> CriarProduto([FromBody] CriarProdutosDto criarProdutosDto)
  {
   if (!ModelState.IsValid)
   {
     _logger.LogWarning("Tentativa de criar produto com dados inválidos: {@ModelState}", ModelState);
     return BadRequest(ModelState);
   }

   var produto = _mapper.Map<Produto>(criarProdutosDto);
   produto.DataCadastro = DateTime.UtcNow;

   _context.Produtos!.Add(produto);
   await _context.SaveChangesAsync();
   _logger.LogInformation("Produto criado com ID: {produtoId}", produto.Id);

   var produtoCriadoDto = _mapper.Map<ProdutoDto>(produto);

   return CreatedAtAction(nameof(GetProdutosPorId), new { id = produtoCriadoDto }, produtoCriadoDto);
  }

  // Atualizar produto
  [HttpPut("{id}")]
  public async Task<IActionResult> AtualizarProduto(int id, [FromBody] AtualizarProdutosDto atualizarProdutosDto)
  {
    if(!ModelState.IsValid)
    {
      _logger.LogWarning("Tentativa de atualizar produto {ProdutoId} com daods inválidos: {@ModelState}", id, ModelState);
      return BadRequest(ModelState);
    }

    var produtoExistente = await _context.Produtos!.FindAsync(id);
    if (produtoExistente == null)
    {
      _logger.LogWarning("Tentativa de atualizar produto com ID: {produtoId} não encontrado.", id);
      return NotFound(new { Message = $"Produto com ID {id} não encontrado." });
    }

    _mapper.Map(atualizarProdutosDto, produtoExistente);

    try
    {
       await _context.SaveChangesAsync();
       _logger.LogInformation("Produto com ID: {ProdutoId} atualizado.", id);
    }
    catch (DbUpdateConcurrencyException ex)
    {
      _logger.LogError(ex, "Erro de concorrência ao atualizar produto com ID: {ProdutoId}.", id);
      if (!await _context.Produtos!.AnyAsync(p => p.Id == id))
      {
        return NotFound(new { Message = $"Produto com ID {id} foi removido por outro usuário." });
      }
      else
      {
        throw; //Re-lança a exceção se não for um problema de não encontrado
      }
    }
      
    return NoContent();
  }

  //Deletar Produto
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeletarProduto(int id)
  {
    var produto = await _context.Produtos!.FindAsync(id);
    if (produto == null)
    {
      _logger.LogWarning("Tentativa de deletar produto com ID: {ProdutoId} não encontrado.", id);
      return NotFound(new { Message = $"Produto com ID {id} não encontrado."});
    }

    _context.Produtos.Remove(produto);
    await _context.SaveChangesAsync();
    _logger.LogInformation("Produto com ID: {ProdutoId} deletado.", id);

    return Ok(new { Message = $"{produto.Nome} com ID: {produto.Id} deletado." });
  }
}




