using Kontatus.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Kontatus.Data.Context
{
    public class KontatusContext : DbContext
    {
        public KontatusContext(DbContextOptions<KontatusContext> options) : base(options) { }

        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<LogUsuario> LogUsuarios { get; set; }
        public DbSet<Arquivo> Arquivos { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Telefone> Telefones { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<ArquivoImportado> ArquivosImportados { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Cachorro> Cachorros { get; set; }
    }
}
