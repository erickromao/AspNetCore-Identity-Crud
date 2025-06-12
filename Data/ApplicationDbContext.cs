// Data/ApplicationDbContext.cs

using Microsoft.EntityFrameworkCore;
// using NomeDoProjetoApi.Models; // Descomente quando tiver entidades na pasta Models
namespace ProdutosAPi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Exemplo de DbSet para uma entidade 'Produto' que será criada posteriormente:
        // public DbSet<Produto> Produtos { get; set; }

        // Adicione outros DbSets para suas entidades aqui.
        // Cada DbSet<TEntity> representa uma tabela no banco de dados.
    }
}

