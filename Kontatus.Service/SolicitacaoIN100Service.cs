using AutoMapper;
using ConsigIntegra.Data.Repository;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsigIntegra.Service
{
    public interface ISolicitacaoIN100Service
    {
        Task<List<SolicitacaoIN100>> GetByUsuarioID(int usuarioID);
    }

    public class SolicitacaoIN100Service : ISolicitacaoIN100Service
    {
        private readonly ISolicitacaoIN100Repository repository;
        private readonly IMapper _mapper;

        public SolicitacaoIN100Service(ISolicitacaoIN100Repository repository, IMapper mapper)
        {
            this.repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SolicitacaoIN100>> GetByUsuarioID(int usuarioID)
        {
            return await repository.GetByUsuarioID(usuarioID);
        }

    }
}
