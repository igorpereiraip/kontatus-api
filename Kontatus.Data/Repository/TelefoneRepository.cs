using Kontatus.Data.Context;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kontatus.Data.Repository
{
    public interface ITelefoneRepository : IRepository<Telefone>
    {
        Task<bool> CreateRange(List<Telefone> telefones);
    }
    public class TelefoneRepository : Repository<Telefone>, ITelefoneRepository
    {
        public TelefoneRepository(KontatusContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }

        public async Task<bool> CreateRange(List<Telefone> telefones)
        {
            await context.Telefones.AddRangeAsync(telefones);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
