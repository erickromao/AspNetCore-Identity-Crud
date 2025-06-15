//using ProdutosAPi.Models;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

public class Produto
{
  [Key]
  public int Id { get; set; }

  [MaxLength(200)]
  public required string Nome { get; set; }

  [MaxLength(1000)]
  public string? Descricao { get; set; }

  public decimal Preco { get; set; }

  public int Estoque { get; set; }

  public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

}
