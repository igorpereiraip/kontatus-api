using Kontatus.Data.Context;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;

namespace Kontatus.Data.Repository
{
    public interface IPerfilRepository : IRepository<Perfil>
    {
    }
    public class PerfilRepository : Repository<Perfil>, IPerfilRepository
    {
        public PerfilRepository(KontatusContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }
    }
}
