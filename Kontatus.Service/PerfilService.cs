using ConsigIntegra.Data.Repository;
using ConsigIntegra.Domain.Entity;

namespace ConsigIntegra.Service
{
    public interface IPerfilService : IService<Perfil>
    {
    }

    public class PerfilService : Service<Perfil>, IPerfilService
    {
        public PerfilService(IPerfilRepository repository)
            : base(repository)
        {
        }
    }
}
