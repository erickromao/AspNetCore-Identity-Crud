namespace ProdutosAPi.DTOs 
{
  public class AtualizarProdutosDto
  {
    public required string Nome { get; set; }
    public String? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
  }
}
