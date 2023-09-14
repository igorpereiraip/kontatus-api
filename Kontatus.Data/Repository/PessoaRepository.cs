using Kontatus.Data.Context;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kontatus.Data.Repository
{
    public interface IPessoaRepository : IRepository<Pessoa>
    {
        Task<bool> CreateRange(List<Pessoa> listPessoa);
    }
    public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(KontatusContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }

        public async Task<bool> CreateRange(List<Pessoa> listPessoa)
        {
            await context.Pessoas.AddRangeAsync(listPessoa);

            await context.SaveChangesAsync();

            return true;
        }

    }
}
