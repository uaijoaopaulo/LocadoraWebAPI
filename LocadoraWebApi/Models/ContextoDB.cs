using System.Data.Entity;

namespace LocadoraWebApi.Models
{
    public class ContextoDB : DbContext
    {
        public DbSet<tb_ClienteCF> Clientes { get; set; }
        public DbSet<tb_FilmeCF> Filmes { get; set; }
        public DbSet<tb_LocacaoCF> Locacoes { get; set; }
    }
}