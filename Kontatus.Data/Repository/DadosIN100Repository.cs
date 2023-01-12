using ConsigIntegra.Data.Context;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsigIntegra.Data.Repository
{
    public interface IDadosIN100Repository : IRepository<DadosIN100>
    {
        Task<DadosIN100> GetByIdSolicitacao(string idSolicitacao);
    }
    public class DadosIN100Repository : Repository<DadosIN100>, IDadosIN100Repository
    {
        public DadosIN100Repository(ConsigIntegraContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }

        public async Task<DadosIN100> GetByIdSolicitacao(string idSolicitacao)
        {
            var lista = new List<DadosIN100>();

            lista = await context.ResultadosIN100.Where(x => x.SolicitacaoID == idSolicitacao).OrderByDescending(log => log.DataCadastro).Take(10000).ToListAsync();
            var dado = lista.FirstOrDefault();


            return dado;
        }
    }
}
