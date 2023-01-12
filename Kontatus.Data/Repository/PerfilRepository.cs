using ConsigIntegra.Data.Context;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;

namespace ConsigIntegra.Data.Repository
{
    public interface IPerfilRepository : IRepository<Perfil>
    {
    }
    public class PerfilRepository : Repository<Perfil>, IPerfilRepository
    {
        public PerfilRepository(ConsigIntegraContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }
    }
}
