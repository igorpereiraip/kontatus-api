using AutoMapper;
using ConsigIntegra.Data.Repository;

namespace ConsigIntegra.Service
{
    public interface ITokenService
    {
    }

    public class TokenService : ITokenService
    {
        private readonly ITokenRepository repository;
        private readonly IMapper _mapper;

        public TokenService(ITokenRepository repository, IMapper mapper)
        {
            this.repository = repository;
            _mapper = mapper;
        }

    }
}
