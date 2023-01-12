using AutoMapper;
using ConsigIntegra.Data.Repository;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsigIntegra.Service
{
    public interface ILogUsuarioService
    {
        void Create(LogUsuarioDTO logUsuario, int loginID, string tagUrl);
        Task<List<LogUsuario>> GetByUsuarioID(int usuarioID);
        Task<List<LogUsuario>> GetByUsuarioIDFiltrado(int usuarioID, string filtro);
    }

    public class LogUsuarioService : ILogUsuarioService
    {
        private readonly ILogUsuarioRepository repository;
        private readonly IMapper _mapper;

        public LogUsuarioService(ILogUsuarioRepository repository, IMapper mapper)
        {
            this.repository = repository;
            _mapper = mapper;
        }

        public void Create(LogUsuarioDTO logUsuario, int loginID, string tagUrl)
        {
            repository.Create(_mapper.Map<LogUsuario>(logUsuario), loginID, tagUrl);
        }

        public async Task<List<LogUsuario>> GetByUsuarioID(int usuarioID)
        {
            return await repository.GetByUsuarioID(usuarioID);
        }
        public async Task<List<LogUsuario>> GetByUsuarioIDFiltrado(int usuarioID, string filtro)
        {
            return await repository.GetByUsuarioIDFiltrado(usuarioID, filtro);
        }

    }
}
