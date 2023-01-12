using AutoMapper;
using ConsigIntegra.Data.Repository;

namespace ConsigIntegra.Service
{
    public interface ISolicitacaoOfflineService
    {
    }

    public class SolicitacaoOfflineService : ISolicitacaoOfflineService
    {
        private readonly ISolicitacaoOfflineRepository repository;
        private readonly IMapper _mapper;

        public SolicitacaoOfflineService(ISolicitacaoOfflineRepository repository, IMapper mapper)
        {
            this.repository = repository;
            _mapper = mapper;
        }

    }
}
