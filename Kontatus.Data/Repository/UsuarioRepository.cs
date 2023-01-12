using ConsigIntegra.Data.Context;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;
using ConsigIntegra.Domain.Enums;
using ConsigIntegra.Helper.Utilitarios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ConsigIntegra.Data.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario> Create(UsuarioDTO usuario);
        Task<Usuario> Update(int id, UsuarioDTO usuario);
        Task<UsuarioDTO> UpdatePassword(string email);
        Task AlterarSenha(string email, string senha, int usuarioID);
        Task<UsuarioDTO> ActivateUsuario(int id);
        Task<Usuario> Obter(int id);
        Task<List<Usuario>> GetUsuarios();
        Task<Usuario> AdicionarSaldoIN100(int id, int valor);
        Task<Usuario> AdicionarSaldoExtrato(int id, int valor);
        Task<Usuario> AdicionarSaldoOffline(int id, int valor);
    }
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ConsigIntegraContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
              incluir = new[] {
                "Logins",
            };
        }

        public async Task<List<Usuario>> GetUsuarios()
        {

            var usuarios = await context.Usuarios.ToListAsync();

            return usuarios;
        }

        public async Task<Usuario> Create(UsuarioDTO usuarioDTO)
        {
            //TODO: Verificar comportamento
            //await VerificarSeExisteUsuarioDoTipo(usuarioDTO);
            var loginExists = await context.Logins.Where(l => l.Email == usuarioDTO.Email && l.Ativo == true).FirstOrDefaultAsync();

            if (loginExists != null)
            {
                throw new Exception("Email já está em uso");
            }

            var usuario = new Usuario
            {
                Administrador = usuarioDTO.Administrador,
                Ativo = true,
                DataCadastro = DateTime.UtcNow,
                Nome = usuarioDTO.Nome,
                SaldoIN100 = usuarioDTO.SaldoIN100,
                SaldoExtrato = usuarioDTO.SaldoExtrato,
                SaldoOffline = usuarioDTO.SaldoOffline,
                LimiteDiario = usuarioDTO.LimiteDiario,
                OfflineIlimitado = usuarioDTO.OfflineIlimitado,
                AcessosSimultaneos = 1,
                ValidadePlano = usuarioDTO.ValidadePlano,
            };

            var login = new Login
            {
                Ativo = true,
                DataCadastro = DateTime.UtcNow,
                Email = usuarioDTO.Email,
                Principal = true,
                Senha = usuarioDTO.Senha,
                TipoLoginID = TipoLogin.ConsigIntegra,
                UsuarioID = usuario.ID,
            };

            usuario.Logins = new List<Login> { login };


            context.Usuarios.Add(usuario);
            context.SaveChanges();

            
            AtribuirLogsNovo(usuario);

            return usuario;
        }



        public async Task<Usuario> Update(int id, UsuarioDTO usuarioDTO)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    var loginExists = await context.Logins.Where(l => l.Email == usuarioDTO.Email && l.UsuarioID != id).FirstOrDefaultAsync();

                    if (loginExists != null)
                    {
                        throw new Exception("Email já está em uso");
                    }

                    var usuario = await context.Usuarios.Include(u => u.Logins).Where(u => u.ID == id).FirstOrDefaultAsync();

                    if (usuario == null)
                        throw new Exception("Usuário não localizado");

                    AtribuirLogsAntigo(usuario);

                    var login = usuario.Logins.Where(x => x.Principal && x.Ativo).FirstOrDefault();

                    if (login == null)
                    {
                        throw new Exception("Ocorreu um erro ao alterar Login do Usuário");
                    }



                    usuario.Administrador = usuarioDTO.Administrador;
                    usuario.DataAlteracao = DateTime.UtcNow;
                    usuario.Nome = usuarioDTO.Nome;
                    usuario.SaldoIN100 = usuarioDTO.SaldoIN100;
                    usuario.SaldoExtrato = usuarioDTO.SaldoExtrato;
                    usuario.SaldoOffline = usuarioDTO.SaldoOffline;
                    usuario.LimiteDiario = usuarioDTO.LimiteDiario;
                    usuario.OfflineIlimitado = usuarioDTO.OfflineIlimitado;
                    usuario.AcessosSimultaneos = usuarioDTO.AcessosSimultaneos;
                    usuario.ValidadePlano = usuarioDTO.ValidadePlano;

                    login.DataAlteracao = DateTime.UtcNow;
                    login.Email = usuarioDTO.Email;
                    login.Senha = usuarioDTO.Senha;

                    usuario.Logins = new List<Login> { login };

                    context.Update(usuario);

                    await context.SaveChangesAsync();

                    AtribuirLogsNovo(usuario);

                    trans.Commit();

                    return usuario;
                }
                catch (Exception)
                {
                    trans.Rollback();

                    return null;
                }
            }
        }

        public override async Task Inactivate(int id)
        {
            Usuario usuario = null;

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    usuario = context.Usuarios.Find(id);

                    if (usuario != null)
                    {
                        usuario.Ativo = false;
                        usuario.DataAlteracao = DateTime.UtcNow;

                        context.Entry(usuario).State = EntityState.Modified;
                        context.SaveChanges();

                        var logins = context.Logins.Where(x => x.UsuarioID == usuario.ID && x.Ativo).ToList();

                        if (logins.Count > 0)
                        {
                            foreach (var l in logins)
                            {
                                l.Ativo = false;
                                l.DataAlteracao = DateTime.UtcNow;

                                context.Entry(l).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                    }

                    await context.SaveChangesAsync();
                    
                    trans.Commit();
                    AtribuirLogsAntigo(usuario);
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }

        public override async Task Activate(int id)
        {
            Usuario usuario = null;

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    usuario = context.Usuarios.Find(id);

                    if (usuario != null)
                    {
                        usuario.Ativo = true;
                        usuario.DataAlteracao = DateTime.UtcNow;

                        context.Entry(usuario).State = EntityState.Modified;
                        context.SaveChanges();

                        var senha = new Random().Next(0, 1000000).ToString("D6");

                        var logins = context.Logins.Where(x => x.UsuarioID == usuario.ID && !x.Ativo).ToList();

                        if (logins.Count > 0)
                        {
                            foreach (var l in logins)
                            {
                                l.Ativo = true;
                                l.DataAlteracao = DateTime.UtcNow;
                                l.Senha = senha;

                                context.Entry(l).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }

                        await context.SaveChangesAsync();
                    }

                    trans.Commit();
                    AtribuirLogsAntigo(usuario);
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }


        public async Task<UsuarioDTO> ActivateUsuario(int id)
        {
            Usuario usuario = null;
            UsuarioDTO usuarioDTO = null;

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    usuario = context.Usuarios.Find(id);

                    if (usuario != null)
                    {
                        usuario.Ativo = true;
                        usuario.DataAlteracao = DateTime.UtcNow;

                        context.Entry(usuario).State = EntityState.Modified;
                        context.SaveChanges();

                        var senha = new Random().Next(0, 1000000).ToString("D6");

                        var logins = context.Logins.Where(x => x.UsuarioID == usuario.ID && !x.Ativo).ToList();

                        if (logins.Count > 0)
                        {
                            foreach (var l in logins)
                            {
                                l.Ativo = true;
                                l.DataAlteracao = DateTime.UtcNow;
                                l.Senha = senha;

                                context.Entry(l).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }

                        await context.SaveChangesAsync();

                        usuarioDTO = new UsuarioDTO { ID = usuario.ID, Email = logins.FirstOrDefault().Email, Nome = usuario.Nome, Senha = senha };
                    }

                    trans.Commit();
                    AtribuirLogsAntigo(usuario);
                    return usuarioDTO;
                }
                catch (Exception)
                {
                    trans.Rollback();

                    return null;
                }
            }
        }

        public async Task<Usuario> AdicionarSaldoIN100(int id, int valor)
        {
            Usuario usuario = null;

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    usuario = context.Usuarios.Find(id);

                    if (usuario != null)
                    {
                        usuario.SaldoIN100 += valor;
                        usuario.DataAlteracao = DateTime.UtcNow;

                        context.Entry(usuario).State = EntityState.Modified;
                        context.SaveChanges();

                    }

                    await context.SaveChangesAsync();

                    trans.Commit();
                    AtribuirLogsAntigo(usuario);
                    return usuario;
                }
                catch (Exception)
                {
                    trans.Rollback();

                    return null;
                }
            }
        }

        public async Task<Usuario> AdicionarSaldoExtrato(int id, int valor)
        {
            Usuario usuario = null;

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    usuario = context.Usuarios.Find(id);

                    if (usuario != null)
                    {
                        usuario.SaldoExtrato += valor;
                        usuario.DataAlteracao = DateTime.UtcNow;

                        context.Entry(usuario).State = EntityState.Modified;
                        context.SaveChanges();

                    }

                    await context.SaveChangesAsync();

                    trans.Commit();
                    AtribuirLogsAntigo(usuario);
                    return usuario;
                }
                catch (Exception)
                {
                    trans.Rollback();

                    return null;
                }
            }
        }

        public async Task<Usuario> AdicionarSaldoOffline(int id, int valor)
        {
            Usuario usuario = null;

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    usuario = context.Usuarios.Find(id);

                    if (usuario != null)
                    {
                        usuario.SaldoOffline += valor;
                        usuario.DataAlteracao = DateTime.UtcNow;

                        context.Entry(usuario).State = EntityState.Modified;
                        context.SaveChanges();

                    }

                    await context.SaveChangesAsync();

                    trans.Commit();
                    AtribuirLogsAntigo(usuario);
                    return usuario;
                }
                catch (Exception)
                {
                    trans.Rollback();

                    return null;
                }
            }
        }


        public async Task<UsuarioDTO> UpdatePassword(string email)
        {
            var usuario = await (from u in context.Usuarios
                                 join l in context.Logins on u.ID equals l.UsuarioID
                                 where u.Ativo && l.Email == email
                                 select u).Include("Logins").SingleOrDefaultAsync();

            if (usuario == null)
                throw new Exception("Usuário não Localizado");

            var senha = new Random().Next(0, 1000000).ToString("D6");

            var login = usuario.Logins.First();

            login.Senha = senha;

            context.Entry(login).State = EntityState.Modified;

            await context.SaveChangesAsync();

            AtribuirLogsAntigo(usuario);

            return new UsuarioDTO { ID = usuario.ID, Senha = senha, Email = email, Nome = usuario.Nome };

        }

        public async Task AlterarSenha(string email, string senha, int usuarioID)
        {
            var usuario = await (from u in context.Usuarios
                                 join l in context.Logins on u.ID equals l.UsuarioID
                                 where u.Ativo && l.Email == email
                                 select u).Include("Logins").FirstOrDefaultAsync();

            if (usuario == null)
                throw new Exception("Usuário não Localizado");

            if (usuario.ID != usuarioID)
            {
                throw new Exception("Não possui permissão para alterar esse usuário");
            }

            var login = usuario.Logins.First();

            login.Senha = senha;

            context.Entry(login).State = EntityState.Modified;
            
            AtribuirLogsAntigo(usuario);

            await context.SaveChangesAsync();
        }

        public Task<Usuario> Obter(int id)
        {
            return context.Usuarios.Where(u => u.ID == id).AsNoTracking().FirstOrDefaultAsync();
        }

        private void AtribuirLogsNovo(Usuario usuario)
        {
            _logUsuarioDTO.RegistroNovo = Conversor.ConverterObjeto(CriarObjetoLog(usuario));
        }

        private void AtribuirLogsAntigo(Usuario usuario)
        {
            _logUsuarioDTO.RegistroAntigo = Conversor.ConverterObjeto(CriarObjetoLog(usuario));
        }


        private object CriarObjetoLog(Usuario usuario)
        {
            return new {
                usuario.Administrador,
                usuario.Ativo,
                usuario.ID,
                usuario.Nome
            };
        }

       
    }
}
