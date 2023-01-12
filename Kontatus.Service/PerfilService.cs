using Kontatus.Data.Repository;
using Kontatus.Domain.Entity;

namespace Kontatus.Service
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
