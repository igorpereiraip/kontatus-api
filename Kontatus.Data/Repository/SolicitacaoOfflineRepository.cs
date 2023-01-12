using Kontatus.Data.Context;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;

namespace Kontatus.Data.Repository
{
    public interface ISolicitacaoOfflineRepository : IRepository<SolicitacaoOffline>
    {
    }
    public class SolicitacaoOfflineRepository : Repository<SolicitacaoOffline>, ISolicitacaoOfflineRepository
    {
        public SolicitacaoOfflineRepository(KontatusContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }

    }
}
