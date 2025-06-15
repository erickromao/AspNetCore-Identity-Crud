using AutoMapper;
using ProdutosAPi.Data;
// using ProdutosAPi.DTOs;
// using ProdutosAPi.Models;
using Microsoft.AspNetCore.Mvc; 
// using Microsoft.EntityFrameworkCore;

public class ExemploController : ControllerBase
{
  private readonly ApplicationDbContext _context;
  private readonly IMapper _mapper;

  public ExemploController(ApplicationDbContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos()
    // {
    //  var produtosEntidades = await _context.Produtos.ToListAsync();
    //  var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtosEntidades);
    //  return Ok(produtosDto);
    // }
}
