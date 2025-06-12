using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Necessário para IdentityDbContext
using Microsoft.EntityFrameworkCore;
using ProdutosAPi.Models;

namespace ProdutosAPi.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Exemplo de DbSet para uma entidade 'Produto' que será criada posteriormente:
        // public DbSet<Produto> Produtos { get; set; }

        // Adicione outros DbSets para suas entidades aqui.
        // Cada DbSet<TEntity> representa uma tabela no banco de dados.
       protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // ESSENCIAL para configurar o schema do Identity

            // Configurações adicionais do modelo do Identity ou da sua aplicação, se necessário.
            // Por exemplo, para customizar nomes de tabelas do Identity:
            // builder.Entity<ApplicationUser>().ToTable("UsuariosApp");
            // builder.Entity<IdentityRole>().ToTable("RolesApp");
            // etc.

            // Se estiver usando a convenção de nomenclatura snake_case globalmente com Npgsql,
            // as tabelas do Identity também serão nomeadas em snake_case (ex: asp_net_users).
            // Se precisar de nomes específicos, pode configurá-los aqui.
        }
    }
}

