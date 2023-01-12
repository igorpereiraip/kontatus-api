using ConsigIntegra.Data.Context;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;

namespace ConsigIntegra.Data.Repository
{
    public interface ISolicitacaoOfflineRepository : IRepository<SolicitacaoOffline>
    {
    }
    public class SolicitacaoOfflineRepository : Repository<SolicitacaoOffline>, ISolicitacaoOfflineRepository
    {
        public SolicitacaoOfflineRepository(ConsigIntegraContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }

    }
}
