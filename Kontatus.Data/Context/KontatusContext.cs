using ConsigIntegra.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ConsigIntegra.Data.Context
{
    public class KontatusContext : DbContext
    {
        public KontatusContext(DbContextOptions<KontatusContext> options) : base(options) { }

        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<LogUsuario> LogUsuarios { get; set; }
        public DbSet<Arquivo> Arquivos { get; set; }
        public DbSet<DadosIN100> ResultadosIN100 { get; set; }
        public DbSet<SolicitacaoIN100> SolicitacoesIN100 { get; set; }
        public DbSet<SolicitacaoOffline> SolicitacoesOffline { get; set; }
        public DbSet<Token> Tokens { get; set; }


    }
}
