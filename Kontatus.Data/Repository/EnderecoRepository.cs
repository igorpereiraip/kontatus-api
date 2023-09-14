using Kontatus.Data.Context;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kontatus.Data.Repository
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<bool> CreateRange(List<Endereco> enderecos);
    }
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(KontatusContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }

        public async Task<bool> CreateRange(List<Endereco> enderecos)
        {
            await context.Enderecos.AddRangeAsync(enderecos);

            await context.SaveChangesAsync();

            return true;
        }

    }
}
