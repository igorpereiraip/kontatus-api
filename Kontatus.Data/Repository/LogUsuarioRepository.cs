
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using ConsigIntegra.Domain.Entity;
using ConsigIntegra.Data.Context;

namespace ConsigIntegra.Data.Repository
{
    public interface ILogUsuarioRepository
    {
        void Create(LogUsuario logUsuario, int loginID, string tagUrl);
        Task<List<LogUsuario>> GetByUsuarioID(int usuarioID);
        Task<List<LogUsuario>> GetByUsuarioIDFiltrado(int usuarioID, string filtro);
    }
    public class LogUsuarioRepository : ILogUsuarioRepository
    {
        protected readonly ConsigIntegraContext context;

        public LogUsuarioRepository(ConsigIntegraContext context)
        {
            this.context = context;
        }

        public void Create(LogUsuario logUsuario, int loginID, string tagUrl)
        {
            var login = context.Logins.Find(loginID);

            logUsuario.UsuarioID = login.UsuarioID;        

            context.LogUsuarios.Add(logUsuario);
            context.SaveChanges();
        }

        public async Task<List<LogUsuario>> GetByUsuarioID(int usuarioID)
        {
            var lista = new List<LogUsuario>();

            if(usuarioID != 0)
            {
                lista = await context.LogUsuarios.Where(x => x.RegistroAfetadoID == usuarioID).Include("Usuario").Include("RegistroAfetado").OrderByDescending(log => log.DataCadastro).Take(10000).ToListAsync();
            }
            else
            {
                lista = await context.LogUsuarios.Include("Usuario").Include("RegistroAfetado").OrderByDescending(log => log.DataCadastro).Take(10000).ToListAsync();

            }

            return lista;
        }

        public async Task<List<LogUsuario>> GetByUsuarioIDFiltrado(int usuarioID, string filtro)
        {
            var lista = new List<LogUsuario>();

            if (usuarioID != 0)
            {
                lista = await context.LogUsuarios.Where(x => x.RegistroAfetadoID == usuarioID).Include("Usuario").Include("RegistroAfetado").OrderByDescending(log => log.DataCadastro).Take(10000).ToListAsync();
            }
            else
            {
                lista = await context.LogUsuarios.Where(x=> x.RegistroAfetado.Nome.ToLower().Contains(filtro))
                    .Include("Usuario").Include("RegistroAfetado").OrderByDescending(log => log.DataCadastro).Take(10000).ToListAsync();

            }

            return lista;
        }

    }
}
