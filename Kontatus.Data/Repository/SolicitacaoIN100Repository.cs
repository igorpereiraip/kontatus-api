using Kontatus.Data.Context;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontatus.Data.Repository
{
    public interface ISolicitacaoIN100Repository : IRepository<SolicitacaoIN100>
    {
        Task<List<SolicitacaoIN100>> GetByUsuarioID(int usuarioID);
    }
    public class SolicitacaoIN100Repository : Repository<SolicitacaoIN100>, ISolicitacaoIN100Repository
    {
        public SolicitacaoIN100Repository(KontatusContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }

        public async Task<List<SolicitacaoIN100>> GetByUsuarioID(int usuarioID)
        {
            var lista = new List<SolicitacaoIN100>();

            if (usuarioID != 0)
            {
                lista = await context.SolicitacoesIN100.Where(x => x.UsuarioID == usuarioID).Include("Usuario").OrderByDescending(log => log.DataCadastro).Take(10000).ToListAsync();
            }
            else
            {
                lista = await context.SolicitacoesIN100.Include("Usuario").OrderByDescending(log => log.DataCadastro).Take(10000).ToListAsync();

            }

            return lista;
        }

    }
}
